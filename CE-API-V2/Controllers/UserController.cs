using AutoMapper;
using CE_API_V2.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CE_API_V2.Models.DTO;
using CE_API_V2.UnitOfWorks.Interfaces;
using Azure.Communication.Email;
using CE_API_V2.Utility;

namespace CE_API_V2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IInputValidationService _inputValidationService;
        private readonly IUserUOW _userUow;
        private readonly IUserInformationExtractor _userInformationExtractor;
        private readonly IMapper _mapper;
        private readonly UserHelper _userHelper;
        
        public UserController(IInputValidationService inputValidationService, 
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

        [HttpPost("me")] 
        [Authorize]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto)
        {
            if (!_inputValidationService.ValidateUser(userDto))
            {
                return BadRequest();
            }

            var idInformation = _userInformationExtractor.GetUserIdInformation(User);

            if (idInformation.UserId.Equals("") ||
                idInformation.UserId.Equals("anonymous"))
            { 
                return BadRequest();
            }

            var user = _userHelper.MapToUserModel(userDto, idInformation);

            var storedUser = await _userUow.StoreUser(user);

            return storedUser is not null ? Ok(_mapper.Map<UserDto>(storedUser)) : BadRequest();
        }

        [HttpPost("request")]
        [Authorize]
        public async Task<IActionResult> RequestAccess([FromBody] AccessRequestDto accessDto)
        {
            if (!_inputValidationService.ValidateAccessRequest(accessDto))
            {
                return BadRequest();
            };

            EmailSendStatus requestStatus = await _userUow.ProcessAccessRequest(accessDto);

            return requestStatus.Equals(EmailSendStatus.Succeeded) ? Ok() : BadRequest();
        }


        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var idInformation = _userInformationExtractor.GetUserIdInformation(User);
            var userId = idInformation.UserId;

            var currentUser = _userUow.GetUser(userId);
            
            var userDto = _mapper.Map<UserDto>(currentUser);

            return currentUser is not null ? Ok(userDto) : BadRequest();
        }
        
    }
}
