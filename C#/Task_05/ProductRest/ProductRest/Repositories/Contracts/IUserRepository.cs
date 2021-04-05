using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductRest.Entities;

namespace ProductRest.Repositories.Contracts
{
    public interface IUserRepository: IBaseRepository<User>
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> GetUserByEmail(string email);
        Task Update(Guid id, User user);
        Task Delete(Guid id);
    }
}