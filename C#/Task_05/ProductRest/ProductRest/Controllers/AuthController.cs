using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductRest.Dto;
using ProductRest.Dto.Auth;
using ProductRest.Infrastructure.Contracts;
using ProductRest.Services.Contracts;


namespace ProductRest.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController: ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;

        public AuthController(ILogger<AuthController> logger, IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        /// <summary> Login. </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /login
        ///     {
        ///         email: ‘test@email.com’,
        ///         password: ‘12345’
        ///     }
        ///
        /// </remarks>
        /// <param name="loginDto">Login dto</param>
        /// <response code="202">Returns the newly created item</response>
        /// <response code="400">One or more validation errors occurred.</response>    
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginDto loginDto)
        {
            if (!await _authService.ValidateUser(loginDto))
                return Unauthorized();

            return Ok(await _authService.Login(loginDto));
        }


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
        [AllowAnonymous]
        [HttpPost("registration")]
        //[ProducesResponseType(201)]
        //[ProducesResponseType(400)]
        public async Task<ActionResult<JwtResult>> Registration(RegistrationDto registrationDto)
        {
            var jwtResult = await _authService.Registration(registrationDto);
            if (jwtResult is null)
                return Unauthorized();
            return Ok(jwtResult);
        }
    }
}