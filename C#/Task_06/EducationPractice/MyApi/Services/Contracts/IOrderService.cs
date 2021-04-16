using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;
using Dto;

namespace Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAll(Guid userId);
        Task<Order> GetOne(Guid orderId, Guid userId);
        Task<Order> Create(CreateOrderDto newOrder, Guid userId);
    }
}