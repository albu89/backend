
using AutoMapper;
using CE_API_V2.Controllers.Filters;
using CE_API_V2.Hasher;
using CE_API_V2.Models.DTO;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.UnitOfWorks;
using CE_API_V2.UnitOfWorks.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;

namespace CE_API_V2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ScoreController : ControllerBase
{
    private readonly IInputValidationService _inputValidationService;
    private readonly IPatientIdHashingUOW _hashingUow;
    private readonly IScoringUOW _scoringUow;
    
    private readonly IScoringTemplateService _scoringTemplateService;
    private readonly IMapper _mapper;

    public ScoreController(IScoringUOW scoringUow,
                           IPatientIdHashingUOW patientIdUow,
                           IInputValidationService inputValidationService,
                           IScoringTemplateService scoringTemplateService,
                           IMapper mapper)
    {
        _hashingUow = patientIdUow;
        _scoringUow = scoringUow;
        _scoringTemplateService = scoringTemplateService;
        _mapper = mapper;
        _inputValidationService = inputValidationService;
    }

    /// <summary>
    ///  Requests a CAD-Score for a specific set of Biomarkers and Patient information.
    /// </summary>
    [HttpPost]
    [Produces("application/json", Type = typeof(ScoringResponseSummary))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<ValidationFailure>))]
    [TypeFilter(typeof(ValidationExceptionFilter))]
    public async Task<IActionResult> PostPatientData([FromBody] ScoringRequestDto value, string? locale)
    {
        if (value == null)
        {
            return BadRequest();
        }
        var patientId = _hashingUow.HashPatientId(value.FirstName, value.LastName, value.DateOfBirth);
        value.FirstName = null;
        value.LastName = null;
        value.DateOfBirth = new DateTime();

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
        var validationResult = _inputValidationService.ScoringRequestIsValid(value);
        if (!validationResult.IsValid)
        {
            throw new ValidationException("Scoringrequest was not valid.", validationResult.Errors);
        }

        var userId = GetUserId();

        
        var requestedScore = await _scoringUow.ProcessScoringRequest(value, userId, patientId);

        var summary = _scoringUow.GetScoreSummary(requestedScore);

        return requestedScore is null ? BadRequest() : Ok(summary);
    }

    [HttpGet]
    [Produces("application/json", Type = typeof(IEnumerable<ScoringHistoryDto>))]
    public IActionResult GetScoringRequests()
    {
        var userId = GetUserId();
        var requests = _scoringUow.RetrieveScoringHistoryForUser(userId);

        return requests is null ? BadRequest() : Ok(requests);
    }

    [HttpGet("patient")]
    [Produces("application/json", Type = typeof(IEnumerable<ScoringHistoryDto>))]
    public IActionResult GetScoringRequests(string name, string lastname, DateTimeOffset dateOfBirth)
    {
        var requests = GetScoringRequestList(name, lastname, dateOfBirth);

        return requests is null ? BadRequest() : Ok(requests);
    }

    [HttpGet("request")]
    [Produces("application/json", Type = typeof(IEnumerable<ScoringResponseSummary>))]
    public IActionResult GetScoringRequest(string name, string lastname, DateTime dateOfBirth, Guid requestId)
    {
        var patientId = _hashingUow.HashPatientId(name, lastname, dateOfBirth);
        // Immediately dereference the values once used
        name = null;
        lastname = null;
        dateOfBirth = new DateTime();

        var userId = GetUserId();
        var scoringResponse = _scoringUow.RetrieveScoringResponse(requestId, userId);

        if (scoringResponse?.Request.PatientId != patientId)
        {
            return BadRequest();
        }

        var scoreSummary = _scoringUow.GetScoreSummary(scoringResponse);

        return scoreSummary is null ? BadRequest() : Ok(scoreSummary);
    }

    private IEnumerable<ScoringHistoryDto> GetScoringRequestList(string name, string lastname, DateTimeOffset dateOfBirth)
    {
        var patientId = _hashingUow.HashPatientId(name, lastname, dateOfBirth);
        // Immediately dereference the values once used
        name = null;
        lastname = null;
        dateOfBirth = new DateTimeOffset();
        var userId = GetUserId();

        return _scoringUow.RetrieveScoringHistoryForPatient(patientId, userId);
    }

    private string GetUserId()
    {
        var userId = User?.Claims?.Any() == true ? User.FindFirstValue(ClaimTypes.NameIdentifier) : "anonymous";
        userId ??= "anonymous";
        return userId;
    }
}