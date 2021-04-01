using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductRest.Dto.Auth;
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
        /// <response code="200">Token is successfully created</response> 
        /// <response code="401">Password or email is invalid</response>
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<string>> Login(LoginDto loginDto)
        {
            if (!await _authService.ValidateUser(loginDto))
                return Unauthorized("Password or email is invalid");
            
            _logger.LogInformation("Token is successfully created");
            return Ok(await _authService.Login(loginDto));
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
        /// <response code="200">User is successfully created</response>
        /// <response code="401">Email is already exist</response>
        [AllowAnonymous]
        [HttpPost("registration")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<JwtResult>> Registration(RegistrationDto registrationDto)
        {
            var jwtResult = await _authService.Registration(registrationDto);
            if (jwtResult is null)
                return Unauthorized("Email is already exist");
            
            _logger.LogInformation("User is successfully created");
            return Ok(jwtResult);
        }
    }
}