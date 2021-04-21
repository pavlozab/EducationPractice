using System.Threading.Tasks;
using Dto;
using JwtAuth;

namespace Services
{
    public interface IAuthService
    {
        Task<bool> ValidateUser(LoginDto loginDto);
        Task<AccessToken> Login(LoginDto loginDto);
        Task<AccessToken> Registration(RegistrationDto registrationDto);
    }
}