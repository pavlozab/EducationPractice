using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductRest.Dto.Order;
using ProductRest.Entities;
using ProductRest.Services.Contracts;

namespace ProductRest.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [ApiController]
    [Route("api/v1/orders")]
    public class OrderController: ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;

        public OrderController(ILogger<OrderController> logger, IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            var userId = GetCurrentUserId();
            return await _orderService.GetAllOrders(userId);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(CreateOrderDto newOrder)
        {
            var userId = GetCurrentUserId();
            return await _orderService.CreateOrder(newOrder, userId);
        }
        
        private Guid GetCurrentUserId()
        {
            return new Guid(ControllerContext.HttpContext.User.Claims.Where(obj => 
                    obj.Type == "UserId")
                .Select(obj => obj.Value).SingleOrDefault());
        }
    }
}