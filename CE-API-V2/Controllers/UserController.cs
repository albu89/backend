using AutoMapper;
using CE_API_V2.Models.DTO;
using CE_API_V2.UnitOfWorks.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CE_API_V2.Services.Interfaces;
using Azure.Communication.Email;
using CE_API_V2.Utility;
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

        [HttpGet(Name = "GetCurrentUser")]
        [Authorize]
        [Produces("application/json", Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCurrentUser()
        {
            var idInformation = _userInformationExtractor.GetUserIdInformation(User);
            var userId = idInformation.UserId;

            var currentUser = _userUow.GetUser(userId);

            var userDto = _mapper.Map<User>(currentUser);

            return currentUser is not null ? Ok(userDto) : NotFound();
        }

        [HttpPost(Name = "CreateCurrentUser")]
        [Authorize]
        [Produces("application/json", Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUser user)
        {
            if (!_inputValidationService.ValidateUser(user))
            {
                return BadRequest();
            }

            var idInformation = _userInformationExtractor.GetUserIdInformation(User);

            if (idInformation.UserId.Equals("") ||
                idInformation.UserId.Equals("anonymous"))
            {
                return BadRequest();
            }

            var userModel = _userHelper.MapToUserModel(user, idInformation);

            var storedUser = await _userUow.StoreUser(userModel);

            return storedUser is not null ? Ok(_mapper.Map<User>(storedUser)) : BadRequest();
        }

        [HttpPost("request", Name = "RequestApplicationAccess")]
        [Authorize]
        [Produces("application/json", Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RequestAccess([FromBody] AccessRequest access)
        {
            if (!_inputValidationService.ValidateAccessRequest(access))
            {
                return BadRequest();
            }
            ;

            EmailSendStatus requestStatus = await _userUow.ProcessAccessRequest(access);

            return requestStatus.Equals(EmailSendStatus.Succeeded) ? Ok() : BadRequest();
        }

        [HttpGet("preferences", Name = "GetPreferences")]
        [Produces("application/json", Type = typeof(BiomarkerOrder))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPreferences()
        {
            try
            {
                var idInformation = _userInformationExtractor.GetUserIdInformation(User);
                var user = _userUow.GetUser(idInformation.UserId);
                var biomarkerOrders = ManualMapper.ToBiomarkerOrder(_userUOW.GetBiomarkerOrders(user.UserId));

                return Ok(biomarkerOrders);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [HttpPost("preferences", Name = "ModifyPreferences")]
        [Produces("application/json", Type = typeof(BiomarkerOrder))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SetPreferences([FromBody] BiomarkerOrder order)
        {
            var idInformation = _userInformationExtractor.GetUserIdInformation(User);
            var user = _userUow.GetUser(idInformation.UserId);

            var orderModelList = ManualMapper.ToBiomarkerOrderModels(order);
            foreach (var orderEntry in orderModelList)
            {
                orderEntry.UserId = user.UserId;
            }

            await _userUOW.StoreOrEditBiomarkerOrder(orderModelList, user.UserId);
            var orderDto = ManualMapper.ToBiomarkerOrder(_userUOW.GetBiomarkerOrders(user.UserId));
            return Ok(orderDto);
        }
        [HttpPatch("preferences", Name = "CreatePreferences")]
        [Produces("application/json", Type = typeof(BiomarkerOrder))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ModifyPreferences([FromBody] BiomarkerOrder order)
        {
            try
            {
                var idInformation = _userInformationExtractor.GetUserIdInformation(User);
                var user = _userUow.GetUser(idInformation.UserId);

                var orderModelList = ManualMapper.ToBiomarkerOrderModels(order);
                foreach (var orderEntry in orderModelList)
                {
                    orderEntry.UserId = user.UserId;
                }

                await _userUOW.StoreOrEditBiomarkerOrder(orderModelList, user.UserId);
                var orderDto = ManualMapper.ToBiomarkerOrder(_userUOW.GetBiomarkerOrders(user.UserId));
                return Ok(orderDto);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}