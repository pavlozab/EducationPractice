using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductRest.Dto.Order;
using ProductRest.Entities;

namespace ProductRest.Services.Contracts
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrders(Guid userId);
        Task<Order> GetOrder(Guid orderId, Guid userId);
        Task<Order> CreateOrder(CreateOrderDto newOrder, Guid userId);
    }
}