using System.Threading.Tasks;
using ProductRest.Dto;
using ProductRest.Dto.Auth;
using ProductRest.Entities;

namespace ProductRest.Services.Contracts
{
    public interface IAuthService
    {
        Task<bool> ValidateUser(LoginDto loginDto);
        Task<JwtResult> Login(LoginDto loginDto);
        Task<JwtResult> Registration(RegistrationDto registrationDto);
    }
}