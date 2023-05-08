using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace CE_API_V2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ScoreController : ControllerBase
{
    [HttpPost]
    public async Task<string> Score(string name)
    {
        return $"Hello {name}!";
    }
}