using System.Security.Claims;
using System.Threading.Tasks;
using ProductRest.Dto.Auth;
using ProductRest.Entities;
using ProductRest.Infrastructure.Contracts;
using ProductRest.Services.Contracts;

namespace ProductRest.Services
{
    public class AuthService: IAuthService
    {
        private readonly IUserService _userService;
        private readonly IJwtAuthManager _jwtAuthManager;

        public AuthService(IUserService userService, IJwtAuthManager jwtAuthManager)
        {
            _userService = userService;
            _jwtAuthManager = jwtAuthManager;
        }

        private JwtResult GetAccessToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role, user.Roles.ToString()),
                new Claim("UserId", user.Id.ToString())
            };
            return _jwtAuthManager.GenerateTokens(user.Email, claims);
        }
        
        public async Task<bool> ValidateUser(LoginDto loginDto)
        {
            if (string.IsNullOrWhiteSpace(loginDto.Email) || string.IsNullOrWhiteSpace(loginDto.Password))
                return false;
            
            var user =  await _userService.GetUserByEmail(loginDto.Email);
            
            return user is not null && BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password);
        }

        public async Task<JwtResult> Login(LoginDto loginDto)
        {
            var user = await _userService.GetUserByEmail(loginDto.Email);
            return GetAccessToken(user);
        }

        public async Task<JwtResult> Registration(RegistrationDto registrationDto)
        {
            var isDuplicateEmail = await _userService.GetUserByEmail(registrationDto.Email);
            if (!(isDuplicateEmail is null))
                return null;
            
            var user = await _userService.CreateUser(registrationDto);
            return GetAccessToken(user);
        }
    }
}