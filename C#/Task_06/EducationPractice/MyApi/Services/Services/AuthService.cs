// using System.Security.Authentication;
// using System.Security.Claims;
// using System.Threading.Tasks;
// using Entities;
// using JwtAuth;
// using Dto;
// using Microsoft.AspNetCore.Identity;
//
// namespace Services
// {
//     public class AuthService: IAuthService
//     {
//         private readonly IUserService _userService;
//         private readonly IJwtAuthManager _jwtAuthManager;
//         private readonly SignInManager<User> _signInManager;
//         private readonly UserManager<User> _userManager;
//         private readonly RoleManager<Role> _roleManager;
//         
//
//         public AuthService(IUserService userService, IJwtAuthManager jwtAuthManager, SignInManager<User> signInManager)
//         {
//             _userService = userService;
//             _jwtAuthManager = jwtAuthManager;
//             _signInManager = signInManager;
//         }
//
//         private string GetAccessToken(User user)
//         {
//             var claims = new[]
//             {
//                 new Claim(ClaimTypes.Email,user.Email),
//                 new Claim(ClaimTypes.Role, user.Roles.ToString()),//user.Roles.ToString()),
//                 new Claim("UserId", user.Id.ToString())
//             };
//             return _jwtAuthManager.GenerateTokens(user.Email, claims);
//         }
//         
//         public async Task<bool> ValidateUser(LoginDto loginDto)
//         {
//             if (string.IsNullOrWhiteSpace(loginDto.Email) || string.IsNullOrWhiteSpace(loginDto.Password))
//                 return false;
//             
//             var user =  await _userService.GetUserByEmail(loginDto.Email);
//
//             return user is not null && BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
//         }
//
//         public async Task<string> Login(LoginDto loginDto)
//         {
//             var user = await _userService.GetUserByEmail(loginDto.Email);
//             return GetAccessToken(user);
//         }
//
//         public async Task<string> Registration(RegistrationDto registrationDto)
//         {
//             var isDuplicateEmail = await _userService.GetUserByEmail(registrationDto.Email);
//             if (!(isDuplicateEmail is null))
//                 throw new AuthenticationException("There is already a user with this email address. Please log in.");
//             
//             var user = await _userService.Create(registrationDto);
//             return GetAccessToken(user);
//         }
//     }
// }

using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using Entities;
using JwtAuth;
using Dto;
using Microsoft.AspNetCore.Identity;

namespace Services
{
    public class AuthService: IAuthService
    {
        private readonly IUserService _userService;
        private readonly IJwtAuthManager _jwtAuthManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        

        public AuthService(IUserService userService, IJwtAuthManager jwtAuthManager, SignInManager<User> signInManager,
            RoleManager<Role> roleManager)
        {
            _userService = userService;
            _jwtAuthManager = jwtAuthManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        private async Task<string> GetAccessToken(User user)
        {
            //await _roleManager.Roles.Where(obj => obj. ); //GetRoleIdAsync(user.)
            var userRoles = await _userService.GetUserRoles(user);
            var claims = new[]
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role, userRoles.Aggregate("", (c, s) => c + $"{s}, ")),
                new Claim("UserId", user.Id.ToString())
            };
            return _jwtAuthManager.GenerateTokens(user.Email, claims);
        }
        
        public async Task<bool> ValidateUser(LoginDto loginDto)
        {
            if (string.IsNullOrWhiteSpace(loginDto.Email) || string.IsNullOrWhiteSpace(loginDto.Password))
                return false;
            
            var user =  await _userService.GetUserByEmail(loginDto.Email);

            return user is not null && BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
        }

        public async Task<string> Login(LoginDto loginDto)
        {
            var user = await _userService.GetUserByEmail(loginDto.Email);
            return await GetAccessToken(user);
        }

        public async Task<string> Registration(RegistrationDto registrationDto)
        {
            await CreateRolesIfNotExists();

            var isDuplicateEmail = await _userService.GetUserByEmail(registrationDto.Email);
            if (isDuplicateEmail != null)
                throw new AuthenticationException("There is already a user with this email address. Please log in.");

            var user = await _userService.Create(registrationDto);

            return await GetAccessToken(user);
        }

        private async Task CreateRolesIfNotExists()
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new Role
                {
                    Name = "Admin",
                    Description = "admin role"
                });
            }
            if (!await _roleManager.RoleExistsAsync("User"))
            {
                await _roleManager.CreateAsync(new Role
                {
                    Name = "User",
                    Description = "user role"
                });
            }
        }
    }
}