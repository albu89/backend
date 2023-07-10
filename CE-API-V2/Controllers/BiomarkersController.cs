using CE_API_V2.Constants;
using CE_API_V2.Models.DTO;
using CE_API_V2.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CE_API_V2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BiomarkersController : ControllerBase
    {
        private readonly IBiomarkersTemplateService _biomarkersTemplateService;

        public BiomarkersController(IBiomarkersTemplateService biomarkersTemplateService)
        {
            _biomarkersTemplateService = biomarkersTemplateService;
        }

        [HttpGet("schema")]
        [Produces("application/json", Type = typeof(IEnumerable<BiomarkerSchemaDto>))]
        public async Task<IActionResult> GetInputFormTemplate(string? locale = null)
        {
            if (string.IsNullOrEmpty(locale))
            {
                locale = LocalizationConstants.DefaultLocale;
            }
            var template = await _biomarkersTemplateService.GetTemplate(locale);

            return template.Any() ? Ok(template) : NotFound();
        }
    }

}