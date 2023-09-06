using System.Net;
using AutoMapper;
using CE_API_V2.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CE_API_V2.Models.DTO;
using CE_API_V2.UnitOfWorks.Interfaces;
using CE_API_V2.Utility;
using Microsoft.AspNetCore.Authorization;
using CE_API_V2.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Swashbuckle.AspNetCore.Annotations;

namespace CE_API_V2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Administrator")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden), SwaggerResponse(403, "Rejects request if user is not an administrator.")]
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

        /// <summary>
        /// Get a list of users 
        /// </summary>
        /// <remarks>
        /// Returns a list of users the current user is allowed to administer.
        /// Only accessible to Administrators of the application.
        /// </remarks>
        /// <returns></returns>
        [HttpGet(Name = "GetUsersList")]
        [Produces("application/json", Type = typeof(IEnumerable<User>)), SwaggerResponse(200, "List of users", typeof(IEnumerable<User>))]
        public async Task<IActionResult> GetAllUsers()
        {
           var userInfo = _userInformationExtractor.GetUserIdInformation(User);
           var userModels = _userUow.GetUsersForAdmin(userInfo);
           var users = _mapper.Map<IEnumerable<User>>(userModels);
            return Ok(users);
        }
        
        /// <summary>
        /// Get specific User by Id
        /// </summary>
        /// <remarks>
        /// Returns a specific users userprofile, selected by the provided UserId.
        /// Only users of the administrators region or company are returned.
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetById")]
        [Produces("application/json", Type = typeof(User)), SwaggerResponse(200, "Returns a UserProfile")]
        [ProducesErrorResponseType(typeof(BadRequest)), SwaggerResponse(400, "Returned when either ScoringRequest does not exist, was created by a different user or patient information does not match.")]
        public async Task<IActionResult> GetUserById(string id)
        {
           var userInfo = _userInformationExtractor.GetUserIdInformation(User);
           var user = _userUow.GetUser(id, userInfo);

           if (user is null || user.TenantID != userInfo.TenantId)
               return BadRequest("Access denied.");
           
            return Ok(_mapper.Map<User>(user));
        }
        
        /// <summary>
        /// Create a new user
        /// </summary>
        /// <remarks>
        /// Creates a new user for the provided userid from active directory.
        /// Users will not be mapped correctly if this userid is not provided.
        /// The correct UserId is the value of the NameIdentifier-Claim on the users OIDC token.
        /// </remarks>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost(Name = "CreateUser")]
        [Produces("application/json", Type = typeof(User)), SwaggerResponse(200, "Returns a UserProfile")]
        public async Task<IActionResult> CreateUserById([FromBody] User user, string activeDirectoryUserId)
        {
            return Ok();
        }

        /// <summary>
        /// Update an existing User
        /// </summary>
        /// <remarks>
        /// Takes in a User and updates their userprofile
        /// </remarks>
        /// <param name="user"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("{id}", Name = "UpdateUserById")]
        [Produces("application/json", Type = typeof(User)), SwaggerResponse(200, "Returns a UserProfile")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUserById([FromBody] CreateUser user, string id)
        {
            var userInfo = _userInformationExtractor.GetUserIdInformation(User);
            var mappedUser = _mapper.Map<UserModel>(user);

            var updatedUser = await _userUow.UpdateUser(id, mappedUser, userInfo);

            var userDto = _mapper.Map<User>(updatedUser);

            return Ok(userDto);
        }

        ///<summary>
        /// Get the preferences of the specified user 
        /// </summary>
        /// <remarks>
        /// Returns the specified users preferences.
        /// Preferences are the order and preferred unit for each biomarker.
        /// Each biomarker is provided as a BiomarkerOrderEntry consisting of unit and order number.
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/preferences", Name = "GetPreferencesForUser")]
        [Produces("application/json", Type = typeof(BiomarkerOrder)), SwaggerResponse(200, "Returns a UserProfile")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPreferences(string id)
        {
            return new OkObjectResult(new User());
        }
        
        /// <summary>
        /// Create new preferences for the specified user
        /// </summary>
        /// <remarks>
        /// Creates a new set of preferences for the specified user.
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost("{id}/preferences", Name = "CreatePreferencesForUser")]
        [Produces("application/json", Type = typeof(BiomarkerOrder)), SwaggerResponse(200, "Returns a UserProfile")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SetPreferences(string id, [FromBody] BiomarkerOrder order)
        {
            return Ok();
        }
        
        /// <summary>
        /// Update specified users preferences
        /// </summary>
        /// <remarks>
        /// Allows for changing the Order and preferred Units per Biomarker for the specified user. 
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPatch("{id}/preferences", Name = "UpdatePreferencesForUser")]
        [Produces("application/json", Type = typeof(BiomarkerOrder)), SwaggerResponse(200, "Returns an Object containing the order of biomarkers and preferred units.")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ModifyPreferences(string id, [FromBody] BiomarkerOrder order)
        {
            return new OkObjectResult(new User());
        }
    }
}
