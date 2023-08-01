using AutoMapper;
using CE_API_V2.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CE_API_V2.Models.DTO;
using CE_API_V2.UnitOfWorks.Interfaces;
using CE_API_V2.Utility;

namespace CE_API_V2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize(Policy = "Administrator")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public class UsersController : ControllerBase
    {
        private readonly IInputValidationService _inputValidationService;
        private readonly IUserUOW _userUow;
        private readonly IUserInformationExtractor _userInformationExtractor;
        private readonly IMapper _mapper;
        private readonly UserHelper _userHelper;
        
        public UsersController(IInputValidationService inputValidationService, 
                              IUserUOW userUow,
                              IUserInformationExtractor userInformationExtractor,
                              IMapper mapper,
                              UserHelper userHelper)
        {
            _inputValidationService = inputValidationService;
            _userUow = userUow;
            _userInformationExtractor = userInformationExtractor;
            _mapper = mapper;
            _userHelper = userHelper;
        }

        [HttpGet(Name = "GetUsersList")]
        [Produces("application/json", Type = typeof(IEnumerable<User>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCurrentUser()
        {
            return Ok(new List<User>());
        }
        
        [HttpGet("{id}", Name = "GetById")]
        [Produces("application/json", Type = typeof(User))]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            return Ok();
        }
        
        [HttpPost("{id}", Name = "CreateUser")]
        [Produces("application/json", Type = typeof(User))]
        public async Task<IActionResult> CreateUserById(Guid id, [FromBody] User user)
        {
            return Ok();
        }

        [HttpGet("{id}/preferences", Name = "GetPreferencesForUser")]
        [Produces("application/json", Type = typeof(BiomarkerOrder))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPreferences(string id)
        {
            return new OkObjectResult(new User());
        }
        
        [HttpPost("{id}/preferences", Name = "CreatePreferencesForUser")]
        [Produces("application/json", Type = typeof(BiomarkerOrder))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SetPreferences(string id, [FromBody] BiomarkerOrder order)
        {
            return Ok();
        }
        
        [HttpPatch("{id}/preferences", Name = "UpdatePreferencesForUser")]
        [Produces("application/json", Type = typeof(BiomarkerOrder))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ModifyPreferences(string id, [FromBody] BiomarkerOrder order)
        {
            return new OkObjectResult(new User());
        }
    }
}
