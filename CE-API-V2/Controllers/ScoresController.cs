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

namespace CE_API_V2.Controllers;

[Route("api/[controller]")]
[ApiController]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[Authorize]
public class ScoresController : ControllerBase
{
    private readonly IInputValidationService _inputValidationService;
    private readonly IPatientIdHashingUOW _hashingUow;
    private readonly IScoringUOW _scoringUow;
    private readonly IUserUOW _userUow;

    public ScoresController(IScoringUOW scoringUow,
        IPatientIdHashingUOW patientIdUow,
        IInputValidationService inputValidationService,
        IUserUOW userUow)
    {
        _hashingUow = patientIdUow;
        _scoringUow = scoringUow;
        _inputValidationService = inputValidationService;
        _userUow = userUow;
    }

    [HttpGet(Name = "GetScoreList")]
    [Produces("application/json", Type = typeof(IEnumerable<SimpleScore>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<ValidationFailure>))]
    public IActionResult GetScoringRequests([FromHeader] string? name = default, [FromHeader] string? lastname = default, [FromHeader] DateTime? dateOfBirth = default)
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


    [HttpGet("{id}", Name = "GetScoreById")]
    [Produces("application/json", Type = typeof(IEnumerable<ScoringResponse>))]
    public IActionResult GetScoringRequest([FromHeader] string name, [FromHeader] string lastname, [FromHeader] DateTime dateOfBirth, Guid id, string locale = "en-GB")
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

        var scoreSummary = _scoringUow.GetScoreSummary(scoringResponse);

        return scoreSummary is null ? BadRequest() : Ok(scoreSummary);
    }

    /// <summary>
    ///  Requests a CAD-Score for a specific set of Biomarkers and Patient information.
    /// </summary>
    [HttpPost("request", Name = "RequestScore")]
    [Produces("application/json", Type = typeof(ScoringResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<ValidationFailure>))]
    [TypeFilter(typeof(ValidationExceptionFilter))]
    public async Task<IActionResult> PostPatientData([FromBody] ScoringRequest value, string? locale)
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
        var currentUser = _userUow.GetUser(userId);
        var validationResult = _inputValidationService.ScoringRequestIsValid(value, currentUser);
        if (!validationResult.IsValid)
        {
            throw new BiomarkersValidationException("Scoringrequest was not valid.", validationResult.Errors, userCulture);
        }



        var requestedScore = await _scoringUow.ProcessScoringRequest(value, userId, patientId);

        var summary = _scoringUow.GetScoreSummary(requestedScore);

        return requestedScore is null ? BadRequest() : Ok(summary);
    }

    private IEnumerable<SimpleScore> GetScoringRequestList(string patientId)
    {
        var userId = UserHelper.GetUserId(User);
        return _scoringUow.RetrieveScoringHistoryForPatient(patientId, userId);
    }
}