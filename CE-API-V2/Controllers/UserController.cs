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
using Microsoft.AspNetCore.RateLimiting;
using CE_API_V2.Utility.CustomAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace CE_API_V2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [UserActive]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden), SwaggerResponse(403, "Rejects request if user has no active account.")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserUOW _userUOW;
        private readonly IInputValidationService _inputValidationService;
        private readonly IUserUOW _userUow;
        private readonly IUserInformationExtractor _userInformationExtractor;
        private readonly IAdministrativeEntitiesUOW _administrativeEntitiesUow;
        private readonly UserHelper _userHelper;

        public UserController(IMapper mapper,
            IUserUOW userUOW,
            IInputValidationService inputValidationService,
            IUserInformationExtractor userInformationExtractor,
            IAdministrativeEntitiesUOW administrativeEntitiesUow,
            UserHelper userHelper)
        {
            _mapper = mapper;
            _userUOW = userUOW;
            _inputValidationService = inputValidationService;
            _userUow = userUOW;
            _userInformationExtractor = userInformationExtractor;
            _administrativeEntitiesUow = administrativeEntitiesUow;
            _userHelper = userHelper;
        }

        /// <summary>Get current User</summary>
        /// <remarks>
        /// Returns the currently authenticated Users Userprofile.
        /// If no Userprofile exists, Status 404 is returned.
        /// If the Userprofile is inactive, Status 403 is returned.
        /// </remarks>
        [HttpGet(Name = "GetCurrentUser")]
        [Produces("application/json", Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult GetCurrentUser()
        {
            var idInformation = _userInformationExtractor.GetUserIdInformation(User);
            var userId = idInformation.UserId;

            var currentUser = _userUow.GetUser(userId, idInformation);

            var userDto = _mapper.Map<User>(currentUser);

            return currentUser is not null ? Ok(userDto) : NoContent();
        }

        /// <summary>Create new User</summary>
        /// <remarks>
        /// Creates a new Userprofile for the currently authenticated User.
        /// If a Userprofile exists already, it returns the current User.
        /// </remarks>
        [HttpPost(Name = "CreateCurrentUser")]
        [AllowInActiveUser]
        [Produces("application/json", Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status400BadRequest), SwaggerResponse(400, "Returns BadRequest if the Userprofile could not be created.")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUser user)
        {
            if (!_inputValidationService.ValidateUser(user))
            {
                return BadRequest("The user could not be validated.");
            }

            var idInformation = _userInformationExtractor.GetUserIdInformation(User);

            if (idInformation.UserId.Equals("") ||
                idInformation.UserId.Equals("anonymous"))
            {
                return BadRequest("Invalid UserId.");
            }

            var userModel = _userHelper.MapToUserModel(user, idInformation);
            userModel = _userHelper.SetActiveStatus(userModel);
            var storedUser = await _userUow.StoreUser(userModel);

            if (!userModel.IsActive)
            {
                await _userUow.ProcessInactiveUserCreation(userModel);
            }

            return storedUser is not null ? Ok(_mapper.Map<User>(storedUser)) : BadRequest("The user could not be created.");
        }

        /// <summary>Update current Users profile</summary>
        /// <remarks>
        /// Used to modify values on the currently authenticated users userprofile.
        /// </remarks>
        /// If the Userprofile is inactive, Status 403 is returned.
        /// <param name="user"></param>
        [HttpPatch(Name = "UpdateUser")]
        [Produces("application/json", Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status400BadRequest), SwaggerResponse(400, "Returns BadRequest if the Userprofile could not be updated.")]
        public async Task<IActionResult> UpdateCurrentUser([FromBody] UpdateUser user)
        {
            var userInfo = _userInformationExtractor.GetUserIdInformation(User);
            var userId = userInfo.UserId;
            UserModel? mappedUser = _mapper.Map<UserModel>(user);

            var updatedUser = await _userUow.UpdateUser(userId, mappedUser, userInfo);

            var userDto = _mapper.Map<User>(updatedUser);

            return Ok(userDto);
        }

        /// <summary>Request access to the application</summary>
        /// <remarks>
        /// Used to request access to the application by unauthenticated users. Sends a notification to Cardio Explorer Administrators.
        /// Does not grant access to the application automatically.
        /// </remarks>
        /// <param name="access"></param>
        [HttpPost("request", Name = "RequestApplicationAccess")]
        [AllowAnonymous]
        [AllowInActiveUser]
        [Produces("application/json", Type = typeof(OkResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest), SwaggerResponse(400, "Returns BadRequest if Email-Service is unavailable.")]
        [EnableRateLimiting("RequestLimitPerMinute")]
        public async Task<IActionResult> RequestAccess([FromBody, SwaggerParameter("Contains all info needed to contact the requester.")] AccessRequest access)
        {
            if (!_inputValidationService.ValidateAccessRequest(access))
            {
                return BadRequest("Invalid Access data.");
            }

            EmailSendStatus requestStatus = await _userUow.ProcessAccessRequest(access);

            return requestStatus.Equals(EmailSendStatus.Succeeded) ? Ok() : BadRequest("Could not process the Access.");
        }

        /// <summary>Get the current users preferences</summary>
        /// <remarks>
        /// Returns the currently authenticated users preferences.
        /// Preferences are the order and preferred unit for each biomarker.
        /// Each biomarker is provided as a BiomarkerOrderEntry consisting of unit and order number.
        /// If the Userprofile is inactive, Status 403 is returned.
        /// </remarks>
        [HttpGet("preferences", Name = "GetPreferences")]
        [Produces("application/json", Type = typeof(BiomarkerOrder))]
        [ProducesResponseType(StatusCodes.Status400BadRequest), SwaggerResponse(400, "Returns bad request result if preferences do not exist.")]
        public async Task<IActionResult> GetPreferences()
        {
            try
            {
                var idInformation = _userInformationExtractor.GetUserIdInformation(User);
                var user = _userUow.GetUser(idInformation.UserId, idInformation);
                var biomarkerOrders = ManualMapper.ToBiomarkerOrder(_userUOW.GetBiomarkerOrders(user.UserId));

                return Ok(biomarkerOrders);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>Create new preferences for current user</summary>
        /// <remarks>
        /// Creates a new set of preferences for the currently authenticated user.
        /// If the Userprofile is inactive, Status 403 is returned.
        /// </remarks>
        [HttpPost("preferences", Name = "CreatePreferences")]
        [Produces("application/json", Type = typeof(BiomarkerOrder))]
        [ProducesResponseType(StatusCodes.Status400BadRequest), SwaggerResponse(400, "Returns bad request result if preferences could not be stored.")]
        public async Task<IActionResult> SetPreferences([FromBody, SwaggerParameter("Contains the preferred Unit and order per biomarker")] BiomarkerOrder order)
        {
            try
            {
                var idInformation = _userInformationExtractor.GetUserIdInformation(User);
                var user = _userUow.GetUser(idInformation.UserId, idInformation);

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

        /// <summary>Update current users preferences</summary>
        /// <remarks>
        /// Allows for changing the Order and preferred Units per Biomarker for the currently authenticated user. 
        /// If the Userprofile is inactive, Status 403 is returned.
        /// </remarks>
        /// <param name="order"></param>
        [HttpPatch("preferences", Name = "ModifyPreferences")]
        [Produces("application/json", Type = typeof(BiomarkerOrder))]
        [ProducesResponseType(StatusCodes.Status400BadRequest), SwaggerResponse(400, "Returns bad request result if preferences could not be stored.")]
        public async Task<IActionResult> ModifyPreferences([FromBody, SwaggerParameter("Contains the preferred Unit and order per biomarker")] BiomarkerOrder order)
        {
            try
            {
                var idInformation = _userInformationExtractor.GetUserIdInformation(User);
                var user = _userUow.GetUser(idInformation.UserId, idInformation);

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