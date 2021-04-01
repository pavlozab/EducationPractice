using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductRest.Dto;
using ProductRest.Dto.Auth;
using ProductRest.Entities;

namespace ProductRest.Services.Contracts
{
    public interface IUserService
    {
        Task<User> GetUserByEmail(string email);
        Task<User> CreateUser(RegistrationDto registrationDto);
        Task<UserResultDto> UpdateRoleOfUser(Guid id, Role newRole);
        Task<IEnumerable<User>> GetAllUsers();
        Task<UserResultDto> DeleteUser(Guid id);
    }
}