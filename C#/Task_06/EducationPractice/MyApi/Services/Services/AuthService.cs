using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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

        public AuthService(IUserService userService, IJwtAuthManager jwtAuthManager)
        {
            _userService = userService;
            _jwtAuthManager = jwtAuthManager;
        }

        private async Task<AccessToken> GetAccessToken(User user)
        { 
            var userRoles = await _userService.GetUserRoles(user);
            
            var claims = new List<Claim>
            {
                new Claim("UserId", user.Id.ToString()),
            };
            
            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            return await _jwtAuthManager.GenerateTokens(claims);
        }
        
        public async Task<bool> ValidateUser(LoginDto loginDto)
        {
            if (string.IsNullOrWhiteSpace(loginDto.Email) || string.IsNullOrWhiteSpace(loginDto.Password))
                return false;
            
            var user =  await _userService.GetUserByEmail(loginDto.Email);

            return user is not null && BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
        }

        public async Task<AccessToken> Login(LoginDto loginDto)
        {
            var user = await _userService.GetUserByEmail(loginDto.Email);
            return await GetAccessToken(user);
        }

        public async Task<AccessToken> Registration(RegistrationDto registrationDto)
        {
            var isDuplicateEmail = await _userService.GetUserByEmail(registrationDto.Email);
            if (isDuplicateEmail != null)
                throw new AuthenticationException("There is already a user with this email address. Please log in.");

            var user = await _userService.Create(registrationDto);

            return await GetAccessToken(user);
        }
    }
}