using System.Threading.Tasks;
using Dto;

namespace Services
{
    public interface IAuthService
    {
        Task<bool> ValidateUser(LoginDto loginDto);
        Task<string> Login(LoginDto loginDto);
        Task<string> Registration(RegistrationDto registrationDto);
    }
}