using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductRest.Dto.Order;
using ProductRest.Entities;

namespace ProductRest.Services.Contracts
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAll(Guid userId);
        Task<Order> GetOne(Guid orderId, Guid userId);
        Task<Order> Create(CreateOrderDto newOrder, Guid userId);
    }
}