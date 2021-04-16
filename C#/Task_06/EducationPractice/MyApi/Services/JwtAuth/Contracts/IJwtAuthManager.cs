using System.Security.Claims;

namespace JwtAuth
{
    public interface IJwtAuthManager
    {
        string GenerateTokens(string email, Claim[] claims);
    }
}