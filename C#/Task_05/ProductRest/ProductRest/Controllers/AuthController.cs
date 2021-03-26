using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductRest.Dtos;
using ProductRest.Entities;


namespace ProductRest.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController
    {
        
        private readonly ILogger<ProductController> _logger;
        
        
        /// <summary>
        /// Registration.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /registration
        ///     {
        ///         firstname: ‘John’,
        ///         lastname: ‘Doe’,
        ///         email: ‘test@email.com’,
        ///         password: ‘12345’
        ///         passwordconfirm
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">One or more validation errors occurred.</response>    
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<RegistrationDto>> Registration(RegistrationDto registrationDto)
        {
            
        }
        
    }
}