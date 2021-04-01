using System.Security.Claims;
using ProductRest.Dto;
using ProductRest.Dto.Auth;

namespace ProductRest.Infrastructure.Contracts
{
    public interface IJwtAuthManager
    {
        JwtResult GenerateTokens(string email, Claim[] claims);
        
    }
}