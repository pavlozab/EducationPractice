using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        /// <summary>
        /// Get all your Orders.
        /// </summary>
        /// <returns>All your orders.</returns>
        /// <response code="200">Orders are successfully returned.</response>
        /// <response code="401">Invalid token.</response>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders()
        {
            var userId = GetCurrentUserId();
            if (userId is null)
                return Unauthorized("Invalid token");
            
            _logger.LogInformation("Orders is successfully returned");
            return Ok(await _orderService.GetAllOrders(new Guid(userId)));
        }

        /// <summary>
        /// Get a specific Order.
        /// </summary>
        /// <param name="id">The id of the order to be retrieved.</param>
        /// <returns>Specific Order.</returns>
        /// <response code="200">Returns order.</response>
        /// <response code="401">Invalid token.</response>
        /// <response code="404">Order hasn't been found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Order>> GetOrder(Guid id)
        {
            var userId = GetCurrentUserId();
            try
            {
                if (userId is null)
                    return Unauthorized("Invalid token");
                
                _logger.LogInformation("Order is successfully returned");
                return Ok(await _orderService.GetOrder(id, new Guid(userId)));
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
        
        /// <summary>
        /// Create a new Order.
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     POST /orders
        ///     {
        ///        "productId": "8a4263bb-fca6-4ab0-a3c9-cf524fe32b8e",
        ///        "amount": 1
        ///     }
        ///
        /// </remarks>
        /// <param name="newOrder">New Order</param>
        /// <returns>New Order</returns>
        /// <response code="201">Returns the newly created order</response>
        /// <response code="403">Not enough product</response>
        /// <response code="404">Product hasn't been found.</response>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> CreateOrder(CreateOrderDto newOrder)
        {
            var userId = GetCurrentUserId();
            if (userId is null)
                return Unauthorized("Invalid token");

            try
            {
                var order = await _orderService.CreateOrder(newOrder, new Guid(userId));
                if (order is null)
                    return StatusCode(403);

                _logger.LogInformation("Order is successfully created");
                return CreatedAtAction(nameof(GetOrder), order); 
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
        
        private string GetCurrentUserId()
        {
            return ControllerContext.HttpContext.User.Claims.Where(obj => 
                    obj.Type == "UserId")
                .Select(obj => obj.Value).SingleOrDefault();
        }
    }
}