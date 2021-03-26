using System;
using System.Threading.Tasks;
using ProductRest.Entities;

namespace ProductRest.Data.Contracts
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        Task CreateUserAsync(User user);
        // Task DeleteUserAsync(Guid id);
        // Task UpdateUserAsync(Guid id ,User user);
    }
}