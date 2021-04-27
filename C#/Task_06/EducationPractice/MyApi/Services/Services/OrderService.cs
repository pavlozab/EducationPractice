using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Data;
using Entities;
using Dto;
using Services.Cache;

namespace Services
{
    public class OrderService: IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IAddressRepository _addressesRepository;
        private readonly ICacheClient _cacheClient;
        private readonly IMapper _mapper;

        public OrderService(
            IOrderRepository orderRepository, 
            IAddressRepository addressesRepository,
            IMapper mapper,
            ICacheClient cacheClient)
        {
            _orderRepository = orderRepository;
            _addressesRepository = addressesRepository;
            _mapper = mapper;
            _cacheClient = cacheClient;
        }
        
        public async Task<IEnumerable<Order>> GetAll(Guid userId)
        {
            return await _orderRepository.GetAll(userId);
        }

        public async Task<OrderResponseDto> GetOne(Guid id, Guid userId)
        {
            var order = await _orderRepository.GetOne(id);
            
            if (order is null)
                throw new KeyNotFoundException("No order found.");

            if (order.Id != userId)
                throw new KeyNotFoundException("No order found.");

            var result = _mapper.Map<OrderResponseDto>(order);
            return result;
        }

        public async Task<OrderResponseDto> Create(CreateOrderDto newOrder, Guid userId)
        {
            var currentProduct = await _addressesRepository.GetOne(newOrder.AddressId);
            
            if (currentProduct is null)
                throw new KeyNotFoundException("No Address found.");

            if (currentProduct.Amount < newOrder.Amount)
                throw new OutOfStockException("Out of stock", newOrder.Amount, currentProduct.Amount);

            var order = _mapper.Map<Order>(newOrder);
            order.UserId = userId;
            order.Date = DateTime.Now;
            
            currentProduct.Amount -= order.Amount;
            await _addressesRepository.Update(currentProduct);
            await _orderRepository.Create(order);
            _cacheClient.RemoveAllCache();

            var result = _mapper.Map<OrderResponseDto>(order);
            return result;
        }
    }

    public class OutOfStockException : ApplicationException
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