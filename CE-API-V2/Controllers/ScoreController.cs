using System.Security.Claims;
using CE_API_V2.Models.DTO;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.Services.Mocks;
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

    public ScoreController(IScoringUOW scoringUow,
        IPatientIdHashingUOW patientIdUow)
    {
        _inputValidationService = new MockedInputValidationService();
        _scoringUow = scoringUow;
        _patientIdUow = patientIdUow;
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
        if (!_inputValidationService.BiomarkersAreValid(value))
        {
            return BadRequest();
        }
        var userId = User?.Claims?.Any() == true ? User.FindFirstValue(ClaimTypes.NameIdentifier) : "anonymous";
        userId ??= "anonymous";

        value.PatientId = new BiomarkerValueDto<string> { Value = patientId };
        var requestedScore = await _scoringUow.ProcessScoringRequest(value, userId);

        return requestedScore is null ? BadRequest() : Ok(requestedScore);
    }

    [HttpGet]
    [Produces("application/json", Type = typeof(IEnumerable<ScoringHistoryDto>))]
    public IActionResult GetScoringRequests()
    {
        var userId = User?.Claims?.Any() == true ? User.FindFirstValue(ClaimTypes.NameIdentifier) : "anonymous";
        userId ??= "anonymous";

        var requests = _scoringUow.RetrieveScoringHistoryForUser(userId);
        return requests is null ? NotFound() : Ok(requests);
    }

    [HttpGet("patient")]
    [Produces("application/json", Type = typeof(IEnumerable<ScoringHistoryDto>))]
    public IActionResult GetScoringRequests(string name, string lastname, DateTime dateOfBirth)
    {
        var userId = User?.Claims?.Any() == true ? User.FindFirstValue(ClaimTypes.NameIdentifier) : "anonymous";
        userId ??= "anonymous";

        var requests = _scoringUow.RetrieveScoringHistoryForPatient(_patientIdUow.GeneratePatientId(name, lastname, dateOfBirth), userId);
        return requests is null ? NotFound() : Ok(requests);
    }
}