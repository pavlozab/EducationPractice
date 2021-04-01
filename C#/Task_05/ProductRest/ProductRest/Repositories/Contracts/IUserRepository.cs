using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductRest.Entities;

namespace ProductRest.Repositories.Contracts
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserAsync(Guid id);
        Task<User> GetUserByEmailAsync(string email);
        Task CreateUserAsync(User user);
        Task UpdateUser(Guid id, User user);
        Task DeleteUser(Guid id);
    }
}