using CE_API_V2.DTO;
using CE_API_V2.Mocks;
using CE_API_V2.Services;
using CE_API_V2.Services.Mocks;
using Microsoft.AspNetCore.Mvc;

namespace CE_API_V2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ScoreController : ControllerBase
{
    //TODO: Just copy/paste for initial test and show case purpose. Needs rework!
    private readonly IInputValidationService _inputValidationService;
    private readonly IAiRequestService _aiRequestService;

    public ScoreController(IAiRequestService aiRequestService)
    {
        _inputValidationService = new MockedInputValidationService();
        _aiRequestService = aiRequestService;
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
        var requestedScore = await _aiRequestService.RequestScore(value);

        return requestedScore is null ? BadRequest() : Ok(requestedScore);
    }
}