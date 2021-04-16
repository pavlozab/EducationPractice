using System.Security.Authentication;
using System.Threading.Tasks;
using Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;

namespace MyApi.Controllers
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
        /// <response code="201">Token is successfully created</response> 
        /// <response code="401">Password or email is invalid</response>
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<string>> Login(LoginDto loginDto)
        {
            if (!await _authService.ValidateUser(loginDto))
                return Unauthorized("Password or email is invalid");

            _logger.LogInformation("Token is successfully created");
            // return StatusCode(201, new TokenResponse(
            //     loginDto.Email, 
            //     await _authService.Login(loginDto))
            // );
            return StatusCode(201, await _authService.Login(loginDto));
        }


        /// <summary> Registration. </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /registration
        ///     {
        ///         firstname: ‘John’,
        ///         lastname: ‘Doe’,
        ///         email: ‘test@email.com’,
        ///         password: ‘12345’
        ///     }
        ///
        /// </remarks>
        /// <response code="201">User is successfully created</response>
        /// <response code="401">Email is already exist</response>
        [AllowAnonymous]
        [HttpPost("registration")]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<string>> Registration(RegistrationDto registrationDto)
        {
            try
            {
                var jwtResult = await _authService.Registration(registrationDto);
                
                _logger.LogInformation("User is successfully created");
                // return StatusCode(201, new TokenResponse(
                //     registrationDto.Email, 
                //     jwtResult)
                // );
                return StatusCode(201, jwtResult);
            }
            catch (AuthenticationException e)
            {
                return Unauthorized(e.Message);
            }
        }
    }
}