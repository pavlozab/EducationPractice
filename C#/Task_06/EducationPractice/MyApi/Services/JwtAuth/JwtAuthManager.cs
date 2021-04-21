using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Config;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;


namespace JwtAuth
{
    public class JwtAuthManager : IJwtAuthManager
    {
        private readonly JwtTokenConfig _jwtTokenConfig;

        public JwtAuthManager(JwtTokenConfig jwtTokenConfig)
        {
            _jwtTokenConfig = jwtTokenConfig;
        }

        public async Task<AccessToken> GenerateTokens(List<Claim> claims)
        {
            var now = DateTime.Now;
            var shouldAddAudienceClaim = string.IsNullOrWhiteSpace(claims?.FirstOrDefault(x =>
                x.Type == JwtRegisteredClaimNames.Aud)?.Value);

            var jwtToken = new JwtSecurityToken(
                _jwtTokenConfig.Issuer,
                shouldAddAudienceClaim ? _jwtTokenConfig.Audience : string.Empty,
                claims,
                expires: now.AddMinutes(_jwtTokenConfig.AccessTokenExpiration),
                signingCredentials: new SigningCredentials(new 
                        SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtTokenConfig.SecretKey)),
                    SecurityAlgorithms.HmacSha256Signature));

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return new AccessToken
            {
                access_token = accessToken
            };
        }
    }

    public class AccessToken
    {
        public string access_token { get; set; }
    }
}