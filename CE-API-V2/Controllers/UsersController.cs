﻿using AutoMapper;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.UnitOfWorks.Interfaces;
using CE_API_V2.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using CE_API_V2.Utility.CustomAnnotations;

namespace CE_API_V2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Administrator")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden), SwaggerResponse(403, "Rejects request if user is not an administrator.")]
    public class UsersController : ControllerBase
    {
        private readonly IUserUOW _userUow;
        private readonly IUserInformationExtractor _userInformationExtractor;
        private readonly IMapper _mapper;
    
        private readonly UserHelper _userHelper;
        
        public UsersController(IUserUOW userUow,
                              IUserInformationExtractor userInformationExtractor,
                              IMapper mapper)
        {
            _userUow = userUow;
            _userInformationExtractor = userInformationExtractor;
            _mapper = mapper;
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
        [UserActive]
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
        [UserActive]
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
        /// Update an existing User
        /// </summary>
        /// <remarks>
        /// Takes in a User and updates their userprofile
        /// </remarks>
        /// <param name="user"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("{id}", Name = "UpdateUserById")]
        [UserActive]
        [Produces("application/json", Type = typeof(User)), SwaggerResponse(200, "Returns a UserProfile")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUserById([FromBody] UpdateUser user, string id)
        {
            var userInfo = _userInformationExtractor.GetUserIdInformation(User);
            var mappedUser = _mapper.Map<UserModel>(user);
            var updatedUser = await _userUow.UpdateUser(id, mappedUser, userInfo);
            var userDto = _mapper.Map<User>(updatedUser);

            return Ok(userDto);
        }
    }
}
