using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductRest.Entities;

namespace ProductRest.Repositories.Contracts
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrders(Guid userId);
        Task<Order> GetOrder(Guid id);
        Task CreateOrder(Order order);
    }
}