using CE_API_V2.Constants;
using CE_API_V2.Models.DTO;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.UnitOfWorks.Interfaces;
using CE_API_V2.Utility;
using CE_API_V2.Utility.CustomAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CE_API_V2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(policy: "CountryPolicy")]
    [UserActive]
    [ProducesResponseType(StatusCodes.Status403Forbidden), SwaggerResponse(403, "Rejects request if user has no active account.")]
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
        /// Get the schema the for ScoringRequest page
        /// </summary>
        /// <remarks>
        /// Returns a list of BiomarkerSchema objects containing all information needed to build a ScoringRequest page.
        /// Each BiomarkerSchema contains a list of Units that the system supports for data entry.
        /// Information is returned in the requested locale if available, otherwise in english.
        /// If the Userprofile is inactive, Status 403 is returned.
        /// </remarks>
        /// <param name="locale" example="de-CH">The requested language and region of the requested resource in IETF BCP 47 format.</param>
        [HttpGet("biomarkers")]
        [Produces("application/json", Type = typeof(IEnumerable<BiomarkerSchema>)), SwaggerResponse(200, "BiomarkerSchema containing all necessary information for creating a ScoringRequest page.", type: typeof(BiomarkerSchema))]
        public async Task<IActionResult> GetInputFormTemplate(string? locale = "en-GB")
        {

            if (string.IsNullOrEmpty(locale))
            {
                locale = LocalizationConstants.DefaultLocale;
            }
            var template = await _biomarkersTemplateService.GetTemplate(locale);
            var idInformation = _userInformationExtractor.GetUserIdInformation(User);
            var user = _userUOW.GetUser(idInformation.UserId, idInformation);
            var userId = user.UserId?.ToString();
            IEnumerable<BiomarkerSchema> schema = _userUOW.OrderTemplate(template, userId);

            return schema.Any() ? Ok(schema) : NotFound();
        }

        ///<summary>Get the schema for the ScoringResponse page</summary>
        /// <remarks>
        /// Returns a ScoreSummary, representing all fields necessary to be displayed on a ScoringResponse page.
        /// All fields are returned in the language specified by locale if translations are available, in english otherwise.
        /// If the Userprofile is inactive, Status 403 is returned.
        /// </remarks>
        /// <param name="locale" example="de-CH">The requested language and region of the requested resource in IETF BCP 47 format.</param> 
        [HttpGet("scoring")]
        [Produces("application/json", Type = typeof(ScoreSchema)), SwaggerResponse(200, "ScoreSchema containing all necessary information for creating a ScoringResponse page.", type: typeof(ScoreSchema))]
        public async Task<IActionResult> GetScoringSchema(string? locale = "en-GB")
        {
            var currentUserId = UserHelper.GetUserId(User);
            var template = await _scoringTemplateService.GetTemplate(currentUserId, locale);

            return template is not null ? Ok(template) : NotFound();
        }
    }

}