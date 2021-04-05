using System;
using System.Threading.Tasks;
using ProductRest.Entities;

namespace ProductRest.Repositories.Contracts
{
    public interface IBaseRepository<T>
    {
        Task<T> Get(Guid id);
        Task Create(T order);
    }
}