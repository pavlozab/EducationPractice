using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using ProductRest.Config;
using ProductRest.Dto;
using ProductRest.Dto.Auth;
using ProductRest.Infrastructure.Contracts;

namespace ProductRest.Infrastructure
{
    public class JwtAuthManager: IJwtAuthManager
    {
        //private readonly ConcurrentDictionary<string, RefreshToken> _usersRefreshTokens; // can store in a database or a distributed cache

        private readonly JwtTokenConfig _jwtTokenConfig;
        private readonly byte[] _secret;

        public JwtAuthManager(JwtTokenConfig jwtTokenConfig)
        {
            _jwtTokenConfig = jwtTokenConfig;
            _secret = Encoding.ASCII.GetBytes(jwtTokenConfig.Secret);
        }
        
        public JwtResult GenerateTokens(string email, Claim[] claims)
        {
            var now = DateTime.Now;
            Console.WriteLine(now);
            var shouldAddAudienceClaim = string.IsNullOrWhiteSpace(claims?.FirstOrDefault(x =>
                x.Type == JwtRegisteredClaimNames.Aud)?.Value);

            var jwtToken = new JwtSecurityToken(
                _jwtTokenConfig.Issuer,
                shouldAddAudienceClaim ? _jwtTokenConfig.Audience : string.Empty,
                claims,
                expires: now.AddMinutes(_jwtTokenConfig.AccessTokenExpiration),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(_secret),
                    SecurityAlgorithms.HmacSha256Signature));

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return new JwtResult
            {
                Email = email,
                AccessToken = accessToken
            };
        }
    }
}