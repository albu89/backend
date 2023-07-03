using System.Security.Claims;
using CE_API_V2.Models.DTO;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.UnitOfWorks;
using CE_API_V2.UnitOfWorks.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CE_API_V2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ScoreController : ControllerBase
{
    private readonly IInputValidationService _inputValidationService;
    private readonly IScoringUOW _scoringUow;
    private readonly IPatientIdHashingUOW _patientIdUow;
    private readonly IValueConversionUOW _valueConversionUow;

    public ScoreController(IScoringUOW scoringUow,
                           IPatientIdHashingUOW patientIdUow,
                           IValueConversionUOW valueConversionUow,
                           IInputValidationService inputValidationService)
    {
        _scoringUow = scoringUow;
        _patientIdUow = patientIdUow;
        _valueConversionUow = valueConversionUow;
        _inputValidationService = inputValidationService;
    }

    [HttpPost]
    [Produces("application/json", Type = typeof(ScoringRequestDto))]
    public async Task<IActionResult> PostPatientData([FromBody] ScoringRequestDto value)
    {
        var patientId = _patientIdUow.GeneratePatientId(value.Firstname, value.Lastname, value.DateOfBirth);

        _ = value.Firstname;
        _ = value.Lastname;
        _ = value.DateOfBirth;

        //POST
        if (!_inputValidationService.ScoringRequestIsValid(value))
        {
            return BadRequest();
        }

        var userId = GetUserId();
        
        var scoringRequest = _valueConversionUow.ConvertToScoringRequest(value, userId, patientId);
                            
        var requestedScore = await _scoringUow.ProcessScoringRequest(scoringRequest, userId, patientId);

        return requestedScore is null ? BadRequest() : Ok(requestedScore);
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
    [Produces("application/json", Type = typeof(IEnumerable<ScoringRequestDto>))]
    public IActionResult GetScoringRequest(string name, string lastname, DateTime dateOfBirth, Guid requestId)
    {
        var requests = GetScoringRequestList(name, lastname, dateOfBirth);
        var specificRequest = requests.FirstOrDefault(x => x.RequestId.Equals(requestId));
            
        return specificRequest is null ? BadRequest() : Ok(specificRequest);
    }

    private IEnumerable<ScoringHistoryDto> GetScoringRequestList(string name, string lastname, DateTimeOffset dateOfBirth)
    {
        var userId = GetUserId();
        var patientId = _patientIdUow.GeneratePatientId(name, lastname, dateOfBirth);

        return _scoringUow.RetrieveScoringHistoryForPatient(patientId, userId);
    }

    private string GetUserId() 
    {
        var userId = User?.Claims?.Any() == true ? User.FindFirstValue(ClaimTypes.NameIdentifier) : "anonymous";
        userId ??= "anonymous";
        return userId;
    }
}