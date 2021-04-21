using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Entities;

namespace JwtAuth
{
    public interface IJwtAuthManager
    {
        Task<AccessToken> GenerateTokens(List<Claim> claims);
    }
}