using CE_API_V2.Controllers.Filters;
using CE_API_V2.Hasher;
using CE_API_V2.Models.DTO;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.UnitOfWorks.Interfaces;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;
using System.Web;
using CE_API_V2.Models.Exceptions;
using CE_API_V2.Utility;
using Microsoft.AspNetCore.Authorization;
using CE_API_V2.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace CE_API_V2.Controllers;

[Route("api/[controller]")]
[ApiController]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
[Authorize]
public class ScoresController : ControllerBase
{
    private readonly IInputValidationService _inputValidationService;
    private readonly IConfiguration _configuration;
    private readonly IPatientIdHashingUOW _hashingUow;
    private readonly IScoringUOW _scoringUow;
    private readonly IUserUOW _userUow;

    public ScoresController(IScoringUOW scoringUow,
        IPatientIdHashingUOW patientIdUow,
        IInputValidationService inputValidationService,
        IConfiguration configuration,
        IUserUOW userUow)
    {
        _hashingUow = patientIdUow;
        _scoringUow = scoringUow;
        _inputValidationService = inputValidationService;
        _configuration = configuration;
        _userUow = userUow;
    }

    /// <summary>
    /// Returns a List of SimpleScores, representing previous ScoringRequests.
    /// Without additional Patient information in the header returns all Requests made by the current user.
    /// With additional Patient Information filters for all requests made for the specified patient.
    /// </summary>
    [HttpGet(Name = "GetScoreList")]
    [Produces("application/json", Type = typeof(IEnumerable<SimpleScore>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<ValidationFailure>))]
    public IActionResult GetScoringRequests([FromHeader, SwaggerParameter("Patients Firstname", Required = false)] string? name = null, [FromHeader, SwaggerParameter("Patients Lastname", Required = false)] string? lastname = null, [FromHeader, SwaggerSchema(Format = "yyyy-MM-dd"), SwaggerParameter("Patients Date of Birth", Required = false)] DateTime? dateOfBirth = null)
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

    /// <summary>
    /// Returns the full response to a specific ScoringRequest.
    /// Only returns the response if patient information and UserID of the current User match.
    /// When specifying a locale the response is translated into the specified language. 
    /// </summary>
    [HttpGet("{id}", Name = "GetScoreById")]
    [Produces("application/json", Type = typeof(ScoringResponse))]
    public IActionResult GetScoringRequest([FromHeader, SwaggerParameter("Patients Firstname", Required = true)] string name, [FromHeader, SwaggerParameter("Patients Lastname", Required = true)] string lastname, [FromHeader, SwaggerSchema(Format = "yyyy-MM-dd"), SwaggerParameter("Patients Date of Birth", Required = true)] DateTime dateOfBirth, Guid id, [FromQuery, SwaggerParameter("Specifies the locale of the response")] string locale = "en-GB")
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(lastname))
        {
            return BadRequest();
        }

        var patientId = _hashingUow.HashPatientId(HttpUtility.UrlDecode(name), HttpUtility.UrlDecode(lastname), dateOfBirth);
        name = null;
        lastname = null;
        dateOfBirth = new DateTime();


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

        CultureInfo.CurrentUICulture = userCulture;

        var userId = UserHelper.GetUserId(User);
        var scoringResponse = _scoringUow.RetrieveScoringResponse(id, userId);

        if (scoringResponse?.Request.PatientId != patientId)
        {
            return BadRequest();
        }

        var scoreSummary = _scoringUow.GetScoreSummary(scoringResponse, scoringResponse.Biomarkers);

        scoreSummary.CanEdit = CalculateIfUpdatePossible(scoringResponse.Request);

        return scoreSummary is null ? BadRequest() : Ok(scoreSummary);
    }

    /// <summary>
    ///  Requests a CAD-Score for a specific set of Biomarkers and Patient information.
    ///  When a locale is provided the response is translated to the specified language.
    ///  The request is validated and invalid requests are rejected. Validation errors are provided in the language specified.
    /// </summary>
    [HttpPost("request", Name = "RequestScore")]
    [Produces("application/json", Type = typeof(ScoringResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<ValidationFailure>))]
    [TypeFilter(typeof(ValidationExceptionFilter))]
    public async Task<IActionResult> PostPatientData([FromBody, SwaggerParameter("Biomarkers for a specific Patient", Required = true)] ScoringRequest value, [FromQuery] string? locale)
    {
        if (value == null || string.IsNullOrEmpty(value.FirstName) || string.IsNullOrEmpty(value.LastName) || value.DateOfBirth is null)
        {
            return BadRequest();
        }

        var patientId = _hashingUow.HashPatientId(value.FirstName, value.LastName, value.DateOfBirth.Value);
        value.FirstName = null;
        value.LastName = null;
        value.DateOfBirth = null;

        //POST
        var userCulture = SetUserCulture(locale);

        CultureInfo.CurrentUICulture = userCulture;

        var userId = UserHelper.GetUserId(User);
        var currentUser = _userUow.GetUser(userId);
        var validationResult = _inputValidationService.ScoringRequestIsValid(value, currentUser);
        if (!validationResult.IsValid)
        {
            throw new BiomarkersValidationException("Scoringrequest was not valid.", validationResult.Errors, userCulture);
        }


        var requestedScore = await _scoringUow.ProcessScoringRequest(value, userId, patientId);

        return requestedScore is null ? BadRequest() : Ok(requestedScore);
    }

    /// <summary>
    /// Generates a new CAD Score for a set of biomarkers for a specific Patient.
    /// Is only successful if Patientdata and UserId match and change request is inside the allowed timeframe. (As indicated by property canEdit on ScoringRequest).
    /// </summary>
    [HttpPut("{scoreId:guid}", Name = "UpdateScore")]
    [Produces("application/json", Type = typeof(ScoringRequest))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<ValidationFailure>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest), SwaggerResponse(400, "Request is rejected if the Edit period has expired.")]
    public async Task<IActionResult> PutPatientData([FromBody,  SwaggerParameter("Biomarkers for a specific Patient", Required = true)] ScoringRequest value, string? locale, [FromRoute, SwaggerParameter("Guid of a previous ScoringRequest")]Guid scoreId)
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
            Console.Error.WriteLine($"Cant retrieve ScoringRequest with given scoreID={scoreId} and userID={userId}, exception: {e.Message}.");
            scoringRequestModel = null;
        }

        if (scoringRequestModel == null || !CalculateIfUpdatePossible(scoringRequestModel))
            return BadRequest("The Edit period of the ScoringRequest has expired. Please create a new ScoringRequest.");
        var requestedScore = await _scoringUow.ProcessScoringRequest(value, userId, patientId, scoringRequestModel.Id);
        return requestedScore is null ? BadRequest() : Ok(requestedScore);

    }

    /// <summary>
    /// calculates: if you can still change the request, based on the given environment variable.
    /// </summary>
    /// <param name="scoringRequestModel"></param>
    /// <returns></returns>
    private bool CalculateIfUpdatePossible(ScoringRequestModel scoringRequestModel)
    {
        var days = _configuration.GetValue<int>("EditPeriodInDays");
        return DateTimeOffset.Now.Subtract(scoringRequestModel.CreatedOn).Days <= days;
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