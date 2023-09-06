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

    public ScoresController(IScoringUOW scoringUow,
        IPatientIdHashingUOW patientIdUow,
        IInputValidationService inputValidationService,
        IConfiguration configuration,
        IUserUOW userUow,
        IScoreSummaryUtility scoreSummaryUtility,
        IUserInformationExtractor userInformationExtractor)
    {
        _hashingUow = patientIdUow;
        _scoringUow = scoringUow;
        _inputValidationService = inputValidationService;
        _configuration = configuration;
        _userUow = userUow;
        _scoreSummaryUtility = scoreSummaryUtility;
        _userInformationExtractor = userInformationExtractor;
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
    [Produces("application/json", Type = typeof(IEnumerable<SimpleScore>)), SwaggerResponse(200, "List of Scores containing id, timestamp of creation, score and risk class.", type: typeof(IEnumerable<SimpleScore>))]
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

            return requests is null ? BadRequest() : Ok(requests);
        }
        var patientId = _hashingUow.HashPatientId(HttpUtility.UrlDecode(name), HttpUtility.UrlDecode(lastname), dateOfBirth.Value);

        requests = GetScoringRequestList(patientId);

        return requests is null ? BadRequest() : Ok(requests);
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
            return BadRequest();
        }

        var patientId = _hashingUow.HashPatientId(HttpUtility.UrlDecode(name), HttpUtility.UrlDecode(lastname), dateOfBirth);
        name = null;
        lastname = null;
        dateOfBirth = new DateTime();

        SetUserCulture(locale);

        var userId = UserHelper.GetUserId(User);
        var request = _scoringUow.RetrieveScoringRequest(scoringRequestId, userId);
        var latestBiomarkers = request.LatestBiomarkers;
        if (request is null || request.PatientId != patientId || latestBiomarkers == null)
        {
            return BadRequest();
        }
        var scoringResponse = latestBiomarkers.Response;
        if (scoringResponse is null)
        {
            var summary = _scoringUow.GetScoringResponse(null, latestBiomarkers);
            return Ok(summary);
        }

        var scoreSummary = _scoringUow.GetScoringResponse(scoringResponse, scoringResponse.Biomarkers);

        scoreSummary.CanEdit = _scoreSummaryUtility.CalculateIfUpdatePossible(scoringResponse.Request);

        return scoreSummary is null ? BadRequest() : Ok(scoreSummary);
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
    [Produces("application/json", Type = typeof(ScoringResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<ValidationFailure>)), SwaggerResponse(400, "The request was malformed or contained invalid values.", type: typeof(IEnumerable<ValidationFailure>))]
    [TypeFilter(typeof(ValidationExceptionFilter))]
    public async Task<IActionResult> PostScoringRequest(
        [FromBody] ScoringRequest scoringRequestValues, 
        [FromQuery] string? locale = "en-GB")
    {
        if (scoringRequestValues == null || string.IsNullOrEmpty(scoringRequestValues.FirstName) || string.IsNullOrEmpty(scoringRequestValues.LastName) || scoringRequestValues.DateOfBirth is null)
        {
            return BadRequest();
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
        var validationResult = _inputValidationService.ScoringRequestIsValid(scoringRequestValues, currentUser);
        if (!validationResult.IsValid)
        {
            throw new BiomarkersValidationException("ScoringRequest was not valid.", validationResult.Errors, userCulture);
        }


        var requestedScore = await _scoringUow.ProcessScoringRequest(scoringRequestValues, userInfo.UserId, patientId);

        return requestedScore is null ? BadRequest() : Ok(requestedScore);
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
    [Produces("application/json", Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest), SwaggerResponse(400,"Returns bad request if patient data is invalid or scoring request can not be stored.")]
    public async Task<IActionResult> PostScoringDraft([FromBody, SwaggerParameter("Biomarkers for a specific Patient", Required = true)] ScoringRequest value)
    {
        if (value == null || string.IsNullOrEmpty(value.FirstName) || string.IsNullOrEmpty(value.LastName) || value.DateOfBirth is null)
        {
            return BadRequest();
        }

        var patientId = _hashingUow.HashPatientId(value.FirstName, value.LastName, value.DateOfBirth.Value);
        value.FirstName = null;
        value.LastName = null;
        value.DateOfBirth = null;
        var userId = UserHelper.GetUserId(User);

        var scoringRequestModel = await _scoringUow.StoreDraftRequest(value, userId, patientId);

        return scoringRequestModel is null ? BadRequest() : Ok(scoringRequestModel.Id);
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
    [Produces("application/json", Type = typeof(ScoringRequest))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<ValidationFailure>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest), SwaggerResponse(400, "Request is rejected if the Edit period has expired.")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<ValidationFailure>)), SwaggerResponse(400, "The request was malformed or contained invalid values.", type: typeof(IEnumerable<ValidationFailure>))]
    public async Task<IActionResult> PutScoringRequest(
        [FromBody] ScoringRequest value, 
        [FromRoute] Guid scoreId, 
        [FromQuery] string? locale = "en-GB")
    {
        if (value == null || string.IsNullOrEmpty(value.FirstName) || string.IsNullOrEmpty(value.LastName) || value.DateOfBirth is null)
        {
            return BadRequest();
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
            scoringRequestModel = null;
        }

        if (scoringRequestModel == null || !_scoreSummaryUtility.CalculateIfUpdatePossible(scoringRequestModel))
            return BadRequest("The edit period of the ScoringRequest has expired.");
        var requestedScore = await _scoringUow.ProcessScoringRequest(value, userId, patientId, scoringRequestModel.Id);
        return requestedScore is null ? BadRequest() : Ok(requestedScore);
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
}