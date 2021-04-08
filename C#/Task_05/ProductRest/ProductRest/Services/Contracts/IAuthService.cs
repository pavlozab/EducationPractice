using System.Threading.Tasks;
using ProductRest.Dto.Auth;

namespace ProductRest.Services.Contracts
{
    public interface IAuthService
    {
        Task<bool> ValidateUser(LoginDto loginDto);
        Task<string> Login(LoginDto loginDto);
        Task<string> Registration(RegistrationDto registrationDto);
    }
}