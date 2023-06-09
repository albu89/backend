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
    //TODO: Just copy/paste for initial test and show case purpose. Needs rework!
    private readonly IInputValidationService _inputValidationService;
    private readonly IAiRequestService _aiRequestService;
    private readonly IScoringUOW _scoringUow;

    public ScoreController(IAiRequestService aiRequestService,
        IScoringUOW scoringUow)
    {
        _inputValidationService = new MockedInputValidationService();
        _aiRequestService = aiRequestService;
        _scoringUow = scoringUow;
    }

    [HttpPost]
    [Produces("application/json", Type = typeof(ScoringRequestDto))]
    public async Task<IActionResult> PostPatientData([FromBody] ScoringRequestDto value)
    {
        //POST
        if (!_inputValidationService.BiomarkersAreValid(value))
        {
            return BadRequest();
        }
        var userId = User?.Claims?.Any() == true ? User.FindFirstValue(ClaimTypes.NameIdentifier) : "anonymous";
        userId ??= "anonymous";
        var requestedScore = await _scoringUow.ProcessScoringRequest(value, userId);

        return requestedScore is null ? BadRequest() : Ok(requestedScore);
    }
}