using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductRest.Dto.Order;
using ProductRest.Entities;
using ProductRest.Repositories.Contracts;
using ProductRest.Services.Contracts;

namespace ProductRest.Services
{
    public class OrderService: IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductsRepository _productsRepository;

        public OrderService(IOrderRepository orderRepository, IProductsRepository productsRepository)
        {
            _orderRepository = orderRepository;
            _productsRepository = productsRepository;
        }
        
        public async Task<IEnumerable<Order>> GetAllOrders(Guid userId)
        {
            return await _orderRepository.GetAllOrders(userId);
        }

        public async Task<Order> GetOrder(Guid id, Guid userId)
        {
            var order = await _orderRepository.GetOrder(id);
            
            if (order is null)
                throw new KeyNotFoundException("No order found.");
            
            if (order.Id != userId)
                return null;
            
            
            return await _orderRepository.GetOrder(id);
        }

        public async Task<Order> CreateOrder(CreateOrderDto newOrder, Guid userId) // Transaction ?
        {
            var currentProduct = await _productsRepository.GetProductAsync(newOrder.ProductId);
            
            if (currentProduct is null)
                throw new KeyNotFoundException("No order found.");

            if (currentProduct.Amount < newOrder.Amount)
                return null; 
            
            Order order = new()
            {
                Id = new Guid(),
                UserId = userId,
                ProductId = newOrder.ProductId,
                Amount = newOrder.Amount,
                Date = DateTime.Now
            };

            currentProduct.Amount -= order.Amount;
            await _productsRepository.UpdateProductAsync(currentProduct);
            await _orderRepository.CreateOrder(order);
            return order;
        }
    }
}