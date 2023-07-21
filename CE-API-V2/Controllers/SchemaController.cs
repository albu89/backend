using System.Security.Claims;
using CE_API_V2.Constants;
using CE_API_V2.Models.DTO;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.UnitOfWorks.Interfaces;
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
        private readonly IUserUOW _userUOW;
        private readonly IUserInformationExtractor _userInformationExtractor;

        public SchemasController(IBiomarkersTemplateService biomarkersTemplateService, IScoringTemplateService scoringTemplateService, IUserUOW userUOW, IUserInformationExtractor userInformationExtractor)
        {
            _biomarkersTemplateService = biomarkersTemplateService;
            _scoringTemplateService = scoringTemplateService;
            _userUOW = userUOW;
            _userInformationExtractor = userInformationExtractor;
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
            var idInformation = _userInformationExtractor.GetUserIdInformation(User);
                var user = _userUOW.GetUser(idInformation.UserId);
            var userId = user.UserId.ToString();
            IEnumerable<BiomarkerSchemaDto> schema = _userUOW.OrderTemplate(template, userId);

            return schema.Any() ? Ok(schema) : NotFound();
        }


        [HttpGet("scoring")]
        [Produces("application/json", Type = typeof(IEnumerable<ScoringSchema>))]
        public async Task<IActionResult> GetScoringSchema(string? locale = null)
        {
            var currentUserId = GetUserId();
            var template = await _scoringTemplateService.GetTemplate(currentUserId, locale);

            return template is not null ? Ok(template) : NotFound();
        }
        
        private string GetUserId()
        {
            var userId = User?.Claims?.Any() == true ? User.FindFirstValue(ClaimTypes.NameIdentifier) : "anonymous";
            userId ??= "anonymous";
            return userId;
        }
    }

}