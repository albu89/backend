using System.Security.Claims;
using CE_API_V2.Constants;
using CE_API_V2.Models.DTO;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.UnitOfWorks.Interfaces;
using CE_API_V2.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CE_API_V2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(policy: "CountryPolicy")]
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

        /// <summary>
        /// Returns a list of BiomarkerSchema objects containing all information needed to build a ScoringRequest page.
        /// Each BiomarkerSchema contains a list of Units that the system supports for data entry.
        /// Information is returned in the requested locale if available, otherwise in english.
        /// </summary>
        [HttpGet("biomarkers")]
        [Produces("application/json", Type = typeof(IEnumerable<BiomarkerSchema>))]
        public async Task<IActionResult> GetInputFormTemplate(string? locale = null)
        {

            if (string.IsNullOrEmpty(locale))
            {
                locale = LocalizationConstants.DefaultLocale;
            }
            var template = await _biomarkersTemplateService.GetTemplate(locale);
            var idInformation = _userInformationExtractor.GetUserIdInformation(User);
            var user = _userUOW.GetUser(idInformation.UserId);
            var userId = user.UserId?.ToString();
            IEnumerable<BiomarkerSchema> schema = _userUOW.OrderTemplate(template, userId);

            return schema.Any() ? Ok(schema) : NotFound();
        }

        /// <summary>
        /// Returns a ScoreSummary, representing all fields necessary to be displayed on a ScoringResponse page.
        /// All fields are returned in the language specified by locale if translations are available, in english otherwise.
        /// </summary>
        [HttpGet("scoring")]
        [Produces("application/json", Type = typeof(ScoreSummary))]
        public async Task<IActionResult> GetScoringSchema(string? locale = null)
        {
            var currentUserId = UserHelper.GetUserId(User);
            var template = await _scoringTemplateService.GetTemplate(currentUserId, locale);

            return template is not null ? Ok(template) : NotFound();
        }
    }

}