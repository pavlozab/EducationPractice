using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductRest.Dto.User;
using ProductRest.Entities;
using ProductRest.Services.Contracts;

namespace ProductRest.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [ApiController]
    [Route("api/v1/user")]
    public class UserController: ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }
        
        /// <summary>
        /// Get all Users. Only for Admin User.
        /// </summary>
        /// <returns>List of User's.</returns>
        /// <response code="200">Returns User's List.</response>
        [HttpGet]
        [Authorize(Roles = nameof(Role.Admin))]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            _logger.LogInformation("Returned all Users");
            return Ok(await _userService.GetAll());
        }
        
        /// <summary>
        /// Update Role of User. Only for Admin User.
        /// </summary>
        /// <param name="id">The id of the user which Role to be updated.</param>
        /// <param name="newRole">New Role</param>
        /// <returns>User with updated Role's.</returns>
        /// <response code="200">Updated user</response>
        [HttpPut("{id}")]
        [Authorize(Roles = nameof(Role.Admin))]
        [ProducesResponseType(200)]
        public async Task<ActionResult<UserResultDto>> UpdateRoleOfUser(Guid id, Role newRole)
        {
            try
            {
                var user = await _userService.UpdateRoleOfUser(id, newRole);
                return Ok(user);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
        
        /// <summary>
        /// Delete User. Only for Admin User.
        /// </summary>
        /// <param name="id">The id of the user to be deleted.</param>
        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            try
            {
                await _userService.Delete(id);
                
                _logger.LogInformation("Deleted user with id: {0}", id);
                return NoContent();
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}