using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<Order> GetOrder(Guid id, Guid UserId)
        {
            // check user 
            
            return await _orderRepository.GetOrder(id);
        }

        public async Task<Order> CreateOrder(CreateOrderDto newOrder, Guid userId)
        {
            var currentProduct = await _productsRepository.GetProductAsync(newOrder.ProductId);
            if (currentProduct is null)
                throw new Exception("Nema takogo"); //FIXME 

            if (currentProduct.Amount < newOrder.Amount)
                throw new Exception("Mnogo ?"); //FIXME 

            Console.WriteLine("2");
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
            Console.WriteLine("3");
            await _orderRepository.CreateOrder(order);
            return order;
        }
    }
}