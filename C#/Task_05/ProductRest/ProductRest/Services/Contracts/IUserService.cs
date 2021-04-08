using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductRest.Dto.Auth;
using ProductRest.Dto.User;
using ProductRest.Entities;

namespace ProductRest.Services.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> GetUserByEmail(string email);
        Task<User> Create(RegistrationDto registrationDto);
        Task<UserResultDto> UpdateRoleOfUser(Guid id, Role newRole);
        Task Delete(Guid id);
    }
}