using AutoMapper;
using CE_API_V2.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CE_API_V2.Models.DTO;
using CE_API_V2.UnitOfWorks.Interfaces;
using CE_API_V2.Utility;
using Microsoft.AspNetCore.Authorization;
using CE_API_V2.Models;

namespace CE_API_V2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Administrator")]
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
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(new List<User>());
        }
        
        [HttpGet("{id}", Name = "GetById")]
        [Produces("application/json", Type = typeof(User))]
        public async Task<IActionResult> GetUserById(string id)
        {
            return Ok();
        }
        
        [HttpPost(Name = "CreateUser")]
        [Produces("application/json", Type = typeof(User))]
        public async Task<IActionResult> CreateUserById([FromBody] User user)
        {
            return Ok();
        }

        [HttpPatch("{id}", Name = "UpdateUserById")]
        [Produces("application/json", Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUserById([FromBody] CreateUser user, string id)
        {
            var mappedUser = _mapper.Map<UserModel>(user);

            var updatedUser = await _userUow.UpdateUser(id, mappedUser);

            var userDto = _mapper.Map<User>(updatedUser);

            return Ok(userDto);
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
