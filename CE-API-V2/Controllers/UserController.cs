using AutoMapper;
using CE_API_V2.Models.DTO;
using CE_API_V2.Models;
using CE_API_V2.UnitOfWorks.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CE_API_V2.Services.Interfaces;
using Azure.Communication.Email;
using CE_API_V2.Utility;
using System.Security.Claims;
using CE_API_V2.Models.Mapping;

namespace CE_API_V2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserUOW _userUOW;
        private readonly IInputValidationService _inputValidationService;
        private readonly IUserUOW _userUow;
        private readonly IUserInformationExtractor _userInformationExtractor;
        private readonly UserHelper _userHelper;

        public UserController(IMapper mapper,
                                IUserUOW userUOW,
                                IInputValidationService inputValidationService,
                                IUserInformationExtractor userInformationExtractor,
                                UserHelper userHelper)
        {
            _mapper = mapper;
            _userUOW = userUOW;
            _inputValidationService = inputValidationService;
            _userUow = userUOW;
            _userInformationExtractor = userInformationExtractor;
            _userHelper = userHelper;
        }

        [HttpPost("preferences")]
        public async Task<IActionResult> StoreOrEditBiomarkerOrder(BiomarkerOrder dtos)
        {
            try
            {
                var idInformation = _userInformationExtractor.GetUserIdInformation(User);
                var user = _userUow.GetUser(idInformation.UserId);

                var order = ManualMapper.ToBiomarkerOrderModels(dtos);
                foreach (var orderEntry in order){
                    orderEntry.UserId = user.UserId;
                }

                await _userUOW.StoreOrEditBiomarkerOrder(order, user.UserId);
                var orderDto = ManualMapper.ToBiomarkerOrder(_userUOW.GetBiomarkerOrders(user.UserId));
                return Ok(orderDto);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("preferences")]
        public async Task<IActionResult> GetBiomarkerOrders()
        {
            try
            {
                var idInformation = _userInformationExtractor.GetUserIdInformation(User);
                var user = _userUow.GetUser(idInformation.UserId);

                var biomarkerOrders = ManualMapper.ToBiomarkerOrder(_userUOW.GetBiomarkerOrders(user.UserId));

                return  Ok(biomarkerOrders);
            }
            catch (Exception)
            {
                return BadRequest();
            }
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
