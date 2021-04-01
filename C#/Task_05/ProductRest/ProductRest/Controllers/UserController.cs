using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductRest.Dto;
using ProductRest.Entities;
using ProductRest.Infrastructure.Contracts;
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
        
        [HttpGet]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<ActionResult<IEnumerable<User>>> GetProducts()
        {
            _logger.LogInformation("Returned all products");
            return Ok(await _userService.GetAllUsers());
        }
        
        [HttpPut("{id}")]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<ActionResult<UserResultDto>> UpdateRoleOfUser(Guid id, Role newRole)
        {
            return Ok(await _userService.UpdateRoleOfUser(id, newRole));
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            if ((await _userService.DeleteUser(id)) is null)
            {
                return NotFound();
            }
                
            _logger.LogInformation("Deleted product with id: {0}", id);
            return NoContent();
        }
    }
}