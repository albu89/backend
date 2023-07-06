using AutoMapper;
using CE_API_V2.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CE_API_V2.Models.DTO;
using CE_API_V2.UnitOfWorks.Interfaces;
using Azure.Communication.Email;

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
        
        public UserController(IInputValidationService inputValidationService, 
                              IUserUOW userUow,
                              IUserInformationExtractor userInformationExtractor,
                              IMapper mapper)
        {
            _inputValidationService = inputValidationService;
            _userUow = userUow;
            _userInformationExtractor = userInformationExtractor;
            _mapper = mapper;
        }

        [HttpPost("me")] 
        [Authorize]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto)
        {
            if (!_inputValidationService.ValidateUser(userDto))
            {
                return BadRequest();
            };

            var idInformation = _userInformationExtractor.GetUserIdInformation(User);

            if (idInformation.UserId.Equals("") ||
                idInformation.UserId.Equals("anonymous")) //Todo: Further validation required?
            { 
                return BadRequest();
            }

            var storedUser = await _userUow.StoreUser(userDto, idInformation); //Todo: Statusmeldung?

            return Ok(_mapper.Map<UserDto>(storedUser));
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
    }
}
