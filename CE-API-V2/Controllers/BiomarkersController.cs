using CE_API_V2.Models.DTO;
using CE_API_V2.Utility;
using Microsoft.AspNetCore.Mvc;

namespace CE_API_V2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BiomarkersController : ControllerBase
    {
        private readonly IBiomarkersTemplateService _biomarkersTemplateService;

        public BiomarkersController(IBiomarkersTemplateService biomarkersTemplateService)
        {
            _biomarkersTemplateService = biomarkersTemplateService;
        }

        [HttpGet("schema")]
        [Produces("application/json", Type = typeof(IEnumerable<BiomarkerSchemaDto>))]
        public async Task<IActionResult> GetInputFormTemplate()
        {
            var template = await _biomarkersTemplateService.GetTemplate();

            return template.Any() ? Ok(template) : NotFound();
        }
    }

}