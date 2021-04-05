using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
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
            try
            {
                var userId = GetCurrentUserId();

                _logger.LogInformation("Orders is successfully returned");
                return Ok(await _orderService.GetAll(new Guid(userId)));
            }
            catch (SecurityTokenValidationException e)
            {
                return Unauthorized(new ProblemDetails
                {
                    Status = StatusCodes.Status401Unauthorized,
                    Title = e.Message,
                    Detail = "You are unauthorised",
                    Instance = HttpContext.Request.Path
                });
            }
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
            try
            {
                var userId = GetCurrentUserId();

                _logger.LogInformation("Order is successfully returned");
                return Ok(await _orderService.GetOne(id, new Guid(userId)));
            }
            catch (SecurityTokenValidationException e)
            {
                return Unauthorized(new ProblemDetails
                {
                    Status = StatusCodes.Status401Unauthorized,
                    Title = e.Message,
                    Detail = "You are unauthorised",
                    Instance = HttpContext.Request.Path
                });
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = e.Message,
                    Detail = "No order found.",
                    Instance = HttpContext.Request.Path
                });
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
            try
            {
                var userId = GetCurrentUserId();
                var order = await _orderService.Create(newOrder, new Guid(userId));

                _logger.LogInformation("Order is successfully created");
                return CreatedAtAction("GetOrder", new { id = order.Id }, order);
            }
            catch (SecurityTokenValidationException e)
            {
                return Unauthorized(new ProblemDetails
                {
                    Status = StatusCodes.Status401Unauthorized,
                    Title = e.Message,
                    Detail = "You are unauthorised",
                    Instance = HttpContext.Request.Path
                });
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = e.Message,
                    Detail = $"No found Product with id: {newOrder.ProductId}",
                    Instance = HttpContext.Request.Path
                });
            }
            catch (UnauthorizedAccessException e) 
            {
                var problemDetail = new ProblemDetails
                {
                    Status = StatusCodes.Status403Forbidden,
                    Title = e.Message,
                    Detail = $"Your current amount is {newOrder.Amount}",
                    Instance = HttpContext.Request.Path
                };
                return new ObjectResult(problemDetail)
                {
                    ContentTypes = { "application/problem+json" },
                    StatusCode = 403,
                };
            }
        }
        
        private string GetCurrentUserId()
        {
            var userId = ControllerContext.HttpContext.User.Claims.Where(obj => 
                    obj.Type == "UserId")
                .Select(obj => obj.Value).SingleOrDefault();
            if (userId is null)
                throw new SecurityTokenValidationException("Invalid token");
            return userId;
        }
    }
}