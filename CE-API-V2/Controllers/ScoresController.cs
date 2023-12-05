using CE_API_V2.Controllers.Filters;
using CE_API_V2.Hasher;
using CE_API_V2.Models.DTO;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.UnitOfWorks.Interfaces;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Web;
using CE_API_V2.Models.Exceptions;
using CE_API_V2.Utility;
using Microsoft.AspNetCore.Authorization;
using CE_API_V2.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.RateLimiting;
using CE_API_V2.Utility.CustomAnnotations;

namespace CE_API_V2.Controllers;

[Route("api/[controller]")]
[ApiController]
[ProducesResponseType(StatusCodes.Status401Unauthorized), SwaggerResponse(401, "The user is not authorized.")]
[ProducesResponseType(StatusCodes.Status403Forbidden), SwaggerResponse(403, "The user is not allowed to access this instance of the api.")]
[Authorize]
[UserActive]
public class ScoresController : ControllerBase
{
    private readonly IInputValidationService _inputValidationService;
    private readonly IConfiguration _configuration;
    private readonly IPatientIdHashingUOW _hashingUow;
    private readonly IScoringUOW _scoringUow;
    private readonly IUserUOW _userUow;
    private readonly IScoreSummaryUtility _scoreSummaryUtility;
    private readonly IUserInformationExtractor _userInformationExtractor;
    private readonly ILogger<ScoresController> _logger;
    private readonly UserHelper _userHelper;

    public ScoresController(IScoringUOW scoringUow,
        IPatientIdHashingUOW patientIdUow,
        IInputValidationService inputValidationService,
        IConfiguration configuration,
        IUserUOW userUow,
        IScoreSummaryUtility scoreSummaryUtility,
        IUserInformationExtractor userInformationExtractor,
        ILogger<ScoresController> logger,
        UserHelper userHelper)
    {
        _hashingUow = patientIdUow;
        _scoringUow = scoringUow;
        _inputValidationService = inputValidationService;
        _configuration = configuration;
        _userUow = userUow;
        _scoreSummaryUtility = scoreSummaryUtility;
        _userInformationExtractor = userInformationExtractor;
        _userHelper = userHelper;
        _logger = logger;
    }

    /// <summary>
    /// Get a List of previous ScoringRequests
    /// </summary>
    /// <remarks>
    /// Returns a List of SimpleScores, representing previous ScoringRequests.
    /// Without additional Patient information in the header returns all Requests made by the current user.
    /// With additional Patient Information filters for all requests made for the specified patient.
    /// </remarks>
    /// <param name="name" example="Paula">The patients first name</param>
    /// <param name="name" example="Thoma">The patients last name</param>
    [HttpGet(Name = "GetScoreList")]
    [UserActive]
    [Produces("application/json", Type = typeof(IEnumerable<SimpleScore>)), 
                                         SwaggerResponse(200, "List of Scores containing id, timestamp of creation, score and risk class.", 
                                         type: typeof(IEnumerable<SimpleScore>))]
    public IActionResult GetScoringRequests(
        [FromHeader, SwaggerParameter("Patients Firstname", Required = false)] string? name = null, 
        [FromHeader, SwaggerParameter("Patients Lastname", Required = false)] string? lastname = null, 
        [FromHeader, SwaggerSchema(Format = "yyyy-MM-dd"), SwaggerParameter("Patients Date of Birth", Required = false)] DateTime? dateOfBirth = null)
    {
        IEnumerable<SimpleScore> requests;

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(lastname) || dateOfBirth is null)
        {
            var userId = UserHelper.GetUserId(User);
            requests = _scoringUow.RetrieveScoringHistoryForUser(userId);

            return requests is null ? BadRequest("No request found for the current User.") : Ok(requests);
        }
        var patientId = _hashingUow.HashPatientId(HttpUtility.UrlDecode(name), HttpUtility.UrlDecode(lastname), dateOfBirth.Value);
        requests = GetScoringRequestList(patientId);

        return requests is null ? BadRequest("No request found for the given parameters.") : Ok(requests);
    }

    /// <summary>Get a full ScoringRequest by ID</summary>
    /// <remarks>
    /// Returns the full response to a specific ScoringRequest.
    /// Only returns the response if patient information and UserID of the current User match.
    /// When specifying a locale the response is translated into the specified language. 
    /// </remarks>
    /// <param name="locale" example="de-CH">The requested language and region of the requested resource in IETF BCP 47 format.</param>
    /// <param name="name" example="Mats">The patients first name.</param>
    /// <param name="lastname" example="Horn">The patients last name.</param>
    /// <param name="dateOfBirth" example="1990-01-01" format="yyyy-MM-dd">The patients date of birth.</param>
    /// <param name="scoringRequestId" example="82231ed6-97a8-4d43-9821-08db9969e7a2">GUID of the ScoringRequest to be loaded.</param>
    [HttpGet("{scoringRequestId}", Name = "GetScoreById")]
    [UserActive]
    [Produces("application/json", Type = typeof(ScoringResponse)), SwaggerResponse(200, "ScoringResponse object contains all CAD Score values and information for interpreting its values.")]
    [ProducesErrorResponseType(typeof(BadRequest)), SwaggerResponse(400, "Returned when either ScoringRequest does not exist, was created by a different user or patient information does not match.")]
    public IActionResult GetScoringRequest(
        [FromHeader] string name, 
        [FromHeader] string lastname, 
        [FromHeader] DateTime dateOfBirth, 
        [FromRoute] Guid scoringRequestId, 
        [FromQuery] string locale = "en-GB")
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(lastname))
        {
            return BadRequest("Invalid first and/or last name.");
        }

        var patientId = _hashingUow.HashPatientId(HttpUtility.UrlDecode(name), HttpUtility.UrlDecode(lastname), dateOfBirth);
        name = null;
        lastname = null;
        dateOfBirth = new DateTime();

        var userCulture = SetUserCulture(locale);
        CultureInfo.CurrentUICulture = userCulture;

        var userId = UserHelper.GetUserId(User);
        var request = _scoringUow.RetrieveScoringRequest(scoringRequestId, userId);

        if (request is null)
        {
            return BadRequest($"No scoring request could be found with this {scoringRequestId}.");
        }
        else if (request.PatientId != patientId)
        {
            return BadRequest("The patient data does not match that of the request.");
        }

        return _scoringUow.RequestIsDraft(request) ? 
            GetDraftSummary(request, patientId) : GetSummary(request, patientId);
    }


    /// <summary>Generate a CAD Score for a set of Biomarkers</summary>
    /// <remarks>
    ///  Requests a CAD-Score for a specific set of Biomarkers and Patient information.
    ///  When a locale is provided the response is translated to the specified language.
    ///  The request is validated and invalid requests are rejected. Validation errors are provided in the language specified.
    /// </remarks>
    /// <param name="locale" example="de-CH">The requested language and region of the requested resource in IETF BCP 47 format.</param>
    /// <param name="scoringRequestValues" required="true">Object containing the biomarker values to request a CAD Score.</param>
    [HttpPost("request", Name = "RequestScore")]
    [UserActive]
    [EnableRateLimiting("RequestLimitPerMinute")]
    [Produces("application/json", Type = typeof(ScoringResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<ValidationFailure>)), SwaggerResponse(400, "The request was malformed or contained invalid values.", type: typeof(IEnumerable<ValidationFailure>))]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests, Type = typeof(IEnumerable<ValidationFailure>)), SwaggerResponse(429, "Too many requests.", type: typeof(IEnumerable<ValidationFailure>))]
    [TypeFilter(typeof(ValidationExceptionFilter))]
    public async Task<IActionResult> PostScoringRequest([FromBody] ScoringRequest scoringRequestValues, [FromQuery] string? locale = "en-GB")
    {
        if (scoringRequestValues == null 
                                 || string.IsNullOrEmpty(scoringRequestValues.FirstName) 
                                 || string.IsNullOrEmpty(scoringRequestValues.LastName) 
                                 || scoringRequestValues.DateOfBirth is null)
        {
            _logger.LogWarning("Tried to request score for invalid patient data.");
            return BadRequest("Invalid patient data.");
        }

        var patientId = _hashingUow.HashPatientId(scoringRequestValues.FirstName, scoringRequestValues.LastName, scoringRequestValues.DateOfBirth.Value);
        scoringRequestValues.FirstName = null;
        scoringRequestValues.LastName = null;
        scoringRequestValues.DateOfBirth = null;

        //POST
        var userCulture = SetUserCulture(locale);
        CultureInfo.CurrentUICulture = userCulture;

        var userInfo = _userInformationExtractor.GetUserIdInformation(User);
        var currentUser = _userUow.GetUser(userInfo.UserId, userInfo);

        if (currentUser is null)
        {
            throw new Exception(); //Todo
        }

        var validationResult = _inputValidationService.ScoringRequestIsValid(scoringRequestValues, currentUser);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Requested invalid scoring request.");
            throw new BiomarkersValidationException("ScoringRequest was not valid.", validationResult.Errors, userCulture);
        }

        var requestedScore = await _scoringUow.ProcessScoringRequest(scoringRequestValues, userInfo.UserId, patientId, currentUser.ClinicalSetting);
        
        return requestedScore is null ? BadRequest("Could not process the scoring request.") : Ok(requestedScore);
    }

    /// <summary>
    ///  Store biomarker values as a draft
    /// </summary>
    /// <remarks>
    /// Stores the provided Biomarkers as a draft to allow for modification in the future. Does not validate the entered biomarkers.
    /// Is only successful if Patient Information is provided.
    /// </remarks>
    /// <param name="value" required="true">Object containing the biomarker values to request a CAD Score.</param>
    [HttpPost(Name = "SaveDraft")]
    [UserActive]
    [Produces("application/json", Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest), SwaggerResponse(400,"Returns bad request if patient data is invalid or scoring request can not be stored.")]
    public async Task<IActionResult> PostScoringDraft([FromBody, SwaggerParameter("Biomarkers for a specific Patient", Required = true)] ScoringRequestDraft value)
    {
        if (value == null 
                  || string.IsNullOrEmpty(value.FirstName) 
                  || string.IsNullOrEmpty(value.LastName) 
                  || value.DateOfBirth is null)
        {
            _logger.LogWarning("Tried to save draft score for invalid patient data.");
            return BadRequest("Invalid patient data.");
        }

        var patientId = _hashingUow.HashPatientId(value.FirstName, value.LastName, value.DateOfBirth.Value);
        value.FirstName = null;
        value.LastName = null;
        value.DateOfBirth = null;
        var userId = UserHelper.GetUserId(User);

        var userInfo = _userInformationExtractor.GetUserIdInformation(User);
        var currentUser = _userUow.GetUser(userInfo.UserId, userInfo);

        var scoringRequestModel = _scoringUow.StoreDraftRequest(value, userId, patientId, currentUser.ClinicalSetting);

        return scoringRequestModel is null ? BadRequest("Could not store the scoring request.") : Ok(scoringRequestModel.Id);
    }

    /// <summary>Create a new CAD Score for a previous ScoringRequest</summary>
    /// <remarks>
    /// Generates a new CAD Score for a set of biomarkers for a specific Patient.
    /// Is only successful if Patientdata and UserId match and change request is inside the allowed timeframe. (As indicated by property canEdit on ScoringRequest).
    /// </remarks>
    /// <param name="locale" example="de-CH">The requested language and region of the requested resource in IETF BCP 47 format.</param>
    /// <param name="scoreId" example="ef227546-c153-40c9-e1e7-08db9cbbf28d" required="true">Guid of a previous ScoringRequest</param>
    /// <param name="value" required="true">Object containing the biomarker values to request a CAD Score.</param>
    [HttpPut("{scoreId:guid}", Name = "UpdateScore")]
    [UserActive]
    [Produces("application/json", Type = typeof(ScoringRequest))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<ValidationFailure>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest), SwaggerResponse(400, "Request is rejected if the Edit period has expired.")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<ValidationFailure>)), SwaggerResponse(400, "The request was malformed or contained invalid values.", type: typeof(IEnumerable<ValidationFailure>))]
    public async Task<IActionResult> PutScoringRequest([FromBody] ScoringRequest value, [FromRoute] Guid scoreId, [FromQuery] string? locale = "en-GB")
    {
        if (value == null 
                  || string.IsNullOrEmpty(value.FirstName) 
                  || string.IsNullOrEmpty(value.LastName) 
                  || value.DateOfBirth is null)
        {
            _logger.LogWarning("Tried to save update score for invalid patient data.");
            return BadRequest("Invalid patient data.");
        }

        var patientId = _hashingUow.HashPatientId(value.FirstName, value.LastName, value.DateOfBirth.Value);
        value.FirstName = null;
        value.LastName = null;
        value.DateOfBirth = null;
        var userId = UserHelper.GetUserId(User);

        var userCulture = SetUserCulture(locale);
        CultureInfo.CurrentUICulture = userCulture;

        ScoringRequestModel? scoringRequestModel;
        try
        {
            scoringRequestModel = _scoringUow.RetrieveScoringRequest(scoreId, userId);
        }
        catch (Exception e)
        {
            _logger.LogCritical("Retrieving scoring request failed with message: {EMessage}", e.Message);
            scoringRequestModel = null;
        }

        if (scoringRequestModel == null || !_scoreSummaryUtility.CalculateIfUpdatePossible(scoringRequestModel))
        {
            _logger.LogWarning($"The user tried to edit a score outside of its edit period. Id: {scoreId}");
            return BadRequest("The edit period of the ScoringRequest has expired.");
        }

        var usedClinicalSetting = scoringRequestModel.ClinicalSetting;
        var requestedScore = await _scoringUow.ProcessScoringRequest(value, userId, patientId, usedClinicalSetting, scoringRequestModel.Id);
        return requestedScore is null ? BadRequest("Could not process the scoring request.") : Ok(requestedScore);
    }
    
    private CultureInfo SetUserCulture(string? locale)
    {
        var userCulture = new CultureInfo("en-GB");
        try
        {
            userCulture = new CultureInfo(locale);
        }
        catch (Exception e)
        {
            // Log exception
            Console.Error.WriteLine(e);
        }
        return userCulture;
    }

    private IEnumerable<SimpleScore> GetScoringRequestList(string patientId)
    {
        var userId = UserHelper.GetUserId(User);
        return _scoringUow.RetrieveScoringHistoryForPatient(patientId, userId);
    }

    private IActionResult GetDraftSummary(ScoringRequestModel request, string patientId)
    {
        var latestBiomarkersDraft = request.LatestBiomarkersDraft;

        if (latestBiomarkersDraft == null)
        {
            return BadRequest("The ScoreRequest does not contain any biomarkers.");
        }

        var scoringResponse = latestBiomarkersDraft.Response;

        if (scoringResponse is null)
        {
            var summary = _scoringUow.GetScoringResponseFromDraft(latestBiomarkersDraft, request.Id);

            if (summary is null)
            {
                BadRequest("A problem occured restoring the scoring resonse");
            }

            return Ok(summary);
        }

        var scoreSummary = _scoringUow.GetScoringResponse(scoringResponse, scoringResponse.Biomarkers, request.Id);

        scoreSummary.CanEdit = _scoreSummaryUtility.CalculateIfUpdatePossible(scoringResponse.Request);

        return scoreSummary is null ? BadRequest("No scoring request was found.") : Ok(scoreSummary);
    }

    private IActionResult GetSummary(ScoringRequestModel request, string patientId)
    {
        var latestBiomarkers = request.LatestBiomarkers;

        if (latestBiomarkers == null)
        {
            return BadRequest("The ScoreRequest does not contain any biomarkers.");
        }

        var scoringResponse = latestBiomarkers.Response;

        if (scoringResponse is null)
        {
            var summary = _scoringUow.GetScoringResponse(null, latestBiomarkers, request.Id);

            if (summary is null)
            {
                return BadRequest("A problem occured restoring the scoring resonse");
            }

            return Ok(summary);
        }

        var scoreSummary = _scoringUow.GetScoringResponse(scoringResponse, scoringResponse.Biomarkers, request.Id);

        scoreSummary.CanEdit = _scoreSummaryUtility.CalculateIfUpdatePossible(scoringResponse.Request);

        return scoreSummary is null ? BadRequest("No scoring request was found.") : Ok(scoreSummary);
    }
}