using System.Security.Claims;
using ProductRest.Dto.Auth;

namespace ProductRest.JwtAuth.Contracts
{
    public interface IJwtAuthManager
    {
        string GenerateTokens(string email, Claim[] claims);
    }
}