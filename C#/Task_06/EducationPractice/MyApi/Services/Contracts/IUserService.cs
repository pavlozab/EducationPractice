using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;
using Dto;

namespace Services
{
    public interface IUserService
    {
        Task<User> GetUserByEmail(string email);
        Task<User> Create(RegistrationDto registrationDto);
        Task<UserResultDto> UpdateRoleOfUser(Guid id, Role newRole);
        Task Delete(Guid id);
        Task<IEnumerable<string>> GetUserRoles(User user);
    }
}