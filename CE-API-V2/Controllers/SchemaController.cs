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
    public class SchemasController : ControllerBase
    {
        private readonly IBiomarkersTemplateService _biomarkersTemplateService;
        private readonly IScoringTemplateService _scoringTemplateService;

        public SchemasController(IBiomarkersTemplateService biomarkersTemplateService, IScoringTemplateService scoringTemplateService)
        {
            _biomarkersTemplateService = biomarkersTemplateService;
            _scoringTemplateService = scoringTemplateService;
        }

        [HttpGet("biomarkers")]
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


        [HttpGet("scoring")]
        [Produces("application/json", Type = typeof(IEnumerable<ScoringSchema>))]
        public async Task<IActionResult> GetScoringSchema(string? locale = null)
        {
            var template = await _scoringTemplateService.GetTemplate(locale);

            return template is not null ? Ok(template) : NotFound();
        }
    }

}