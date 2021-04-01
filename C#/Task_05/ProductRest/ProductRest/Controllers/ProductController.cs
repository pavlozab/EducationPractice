using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductRest.Dto;
using ProductRest.Entities;
using ProductRest.Models;
using ProductRest.Responses;
using ProductRest.Services.Contracts;

namespace ProductRest.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [ApiController]
    [Route("api/v1/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService service, ILogger<ProductController> logger)
        {
            _service = service;
            _logger = logger;
        }
        
        /// <summary>
        /// Get Products.
        /// </summary>
        /// <response code="200">Returns Product List</response>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<PagedResponse<Product>>> GetProducts([FromQuery]QueryParametersModel filter)
        {
            _logger.LogInformation("Returned all products");
            return Ok(await _service.GetProducts(filter));
        }

        /// <summary>
        /// Get a specific Product.
        /// </summary>
        /// <param name="id">The id of the item to be retrieved</param>
        /// <response code="200">Returns a specific Product</response>
        /// <response code="404">Product hasn't been found.</response>
        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
            var product = await _service.GetProduct(id);

            if (product is null)
                return NotFound();

            _logger .LogInformation("Returned product with id: {0}", id);
            return Ok(product);
        }

        /// <summary>
        /// Create a Product.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /products
        ///     {
        ///        "addressLine": "new address",
        ///        "postalCode": "12345",
        ///        "country": "new country",
        ///        "city": "new city",
        ///        "faxNumber": "+380111111111",
        ///        "phoneNumber": "+380222222222",
        ///        "amount": 4
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="405">Method is not allowed</response>    
        [HttpPost]
        [Authorize(Roles = nameof(Role.Admin))]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Product>> CreateProduct(CreateProductDto productDto)
        {
            var product = await _service.CreateProduct(productDto);
            _logger.LogInformation("Create a Product");
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        /// <summary>
        /// Update a specific Product.
        /// </summary>
        /// <param name="id">The id of the item to be retrieved</param>
        /// <param name="productDto">New product</param>
        /// <response code="204">Updated product</response>
        /// <response code="404">Product hasn't been found.</response>
        [HttpPut("{id}")]
        [Authorize(Roles = nameof(Role.Admin))]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateProduct(Guid id, UpdateProductDto productDto)
        {
            if ((await  _service.UpdateProduct(id, productDto)) is null)
                return NotFound();

            _logger.LogInformation("Updated product with id: {0}", id);
            return NoContent();
        }

        /// <summary>
        /// Deletes a specific Product.
        /// </summary>
        /// <param name="id">The id of the item to be deleted</param>
        /// <response code="204">Deleted product</response>
        /// <response code="404">Product hasn't been found.</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(Role.Admin))]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            if ((await _service.DeleteProduct(id)) is null)
            {
                return NotFound();
            }
                
            _logger.LogInformation("Deleted product with id: {0}", id);
            return NoContent();
        }
    }
}