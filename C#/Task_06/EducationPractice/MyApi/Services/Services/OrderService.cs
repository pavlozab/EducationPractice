using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Entities;
using Dto;

namespace Services
{
    public class OrderService: IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IAddressRepository _addressesRepository;

        public OrderService(IOrderRepository orderRepository, IAddressRepository addressesRepository)
        {
            _orderRepository = orderRepository;
            _addressesRepository = addressesRepository;
        }
        
        public async Task<IEnumerable<Order>> GetAll(Guid userId)
        {
            return await _orderRepository.GetAll(userId);
        }

        public async Task<Order> GetOne(Guid id, Guid userId)
        {
            var order = await _orderRepository.GetOne(id);
            
            if (order is null)
                throw new KeyNotFoundException("No order found.");

            if (order.Id != userId)
                throw new KeyNotFoundException("No order found.");

            return await _orderRepository.GetOne(id);
        }

        public async Task<Order> Create(CreateOrderDto newOrder, Guid userId) // Transaction ?
        {
            var currentProduct = await _addressesRepository.GetOne(newOrder.AddressId);
            
            if (currentProduct is null)
                throw new KeyNotFoundException("No order found.");

            if (currentProduct.Amount < newOrder.Amount)
                throw new OutOfStockException("Out of stock", newOrder.Amount, currentProduct.Amount);
            
            Order order = new()
            {
                Id = new Guid(),
                UserId = userId,
                AddressId = newOrder.AddressId,
                Amount = newOrder.Amount,
                Date = DateTime.Now
            };

            currentProduct.Amount -= order.Amount;
            await _addressesRepository.Update(currentProduct);
            await _orderRepository.Create(order);
            return order;
        }
    }

    public class OutOfStockException : Exception
    {
        public decimal OrderAmount { get; set; }
        public decimal ProductAmount { get; set; }

        public OutOfStockException(string message, decimal orderAmount, decimal productAmount) 
            : base(message)
        {
            OrderAmount = orderAmount;
            ProductAmount = productAmount;
        }
    }
}