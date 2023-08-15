using AutoMapper;
using CE_API_V2.Models.DTO;
using CE_API_V2.UnitOfWorks.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CE_API_V2.Services.Interfaces;
using Azure.Communication.Email;
using CE_API_V2.Models;
using CE_API_V2.Utility;
using CE_API_V2.Models.Mapping;
using Swashbuckle.AspNetCore.Annotations;

namespace CE_API_V2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

        /// <summary>
        /// Returns the currently authenticated Users Userprofile
        /// </summary>
        [HttpGet(Name = "GetCurrentUser")]
        [Produces("application/json", Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCurrentUser()
        {
            var idInformation = _userInformationExtractor.GetUserIdInformation(User);
            var userId = idInformation.UserId;

            var currentUser = _userUow.GetUser(userId);

            var userDto = _mapper.Map<User>(currentUser);

            return currentUser is not null ? Ok(userDto) : NotFound();
        }

        /// <summary>
        /// Creates a new Userprofile for the currently authenticated User.
        /// If a Userprofile exists already, it returns the current User.
        /// </summary>
        [HttpPost(Name = "CreateCurrentUser")]
        [Produces("application/json", Type = typeof(User))]
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

        /// <summary>
        /// Used to modify values on the currently authenticated users userprofile.
        /// </summary>
        /// <param name="user"></param>
        [HttpPatch(Name = "UpdateUser")]
        [Produces("application/json", Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCurrentUser([FromBody] CreateUser user)
        {
            var userId = _userInformationExtractor.GetUserIdInformation(User).UserId;
            UserModel? mappedUser = _mapper.Map<UserModel>(user);

            var updatedUser = await _userUow.UpdateUser(userId, mappedUser);

            var userDto = _mapper.Map<User>(updatedUser);

            return Ok(userDto);
        }

        /// <summary>
        /// Used to request access to the application by unauthenticated users. Sends a notification to Cardio Explorer Administrators.
        /// Does not grant access to the application automatically.
        /// </summary>
        /// <param name="access"></param>
        [HttpPost("request", Name = "RequestApplicationAccess")]
        [AllowAnonymous]
        [Produces("application/json", Type = typeof(OkResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RequestAccess([FromBody,  SwaggerParameter("Contains all info needed to contact the requester.")] AccessRequest access)
        {
            if (!_inputValidationService.ValidateAccessRequest(access))
            {
                return BadRequest();
            }

            EmailSendStatus requestStatus = await _userUow.ProcessAccessRequest(access);

            return requestStatus.Equals(EmailSendStatus.Succeeded) ? Ok() : BadRequest();
        }
        
        /// <summary>
        /// Returns the currently authenticated users Preferences.
        /// BiomarkerOrder consists of multiple BiomarkerOrderEntries, one per Biomarker and allows setting a preferred Unit and Order for the User.
        /// </summary>
        [HttpGet("preferences", Name = "GetPreferences")]
        [Produces("application/json", Type = typeof(BiomarkerOrder))]
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

        /// <summary>
        /// Creates a new set of preferences for the currently authenticated user.
        /// </summary>
        [HttpPost("preferences", Name = "CreatePreferences")]
        [Produces("application/json", Type = typeof(BiomarkerOrder))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SetPreferences([FromBody, SwaggerParameter("Contains the preferred Unit and order per biomarker")] BiomarkerOrder order)
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

        /// <summary>
        /// Allows for changing the Order and preferred Units per Biomarker for the currently authenticated user. 
        /// </summary>
        /// <param name="order"></param>
        [HttpPatch("preferences", Name = "ModifyPreferences")]
        [Produces("application/json", Type = typeof(BiomarkerOrder))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ModifyPreferences([FromBody,  SwaggerParameter("Contains the preferred Unit and order per biomarker")] BiomarkerOrder order)
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