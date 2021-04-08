using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductRest.Entities;

namespace ProductRest.Repositories.Contracts
{
    public interface IOrderRepository: IBaseRepository<Order>
    {
        Task<IEnumerable<Order>> GetAll(Guid userId);
    }
}