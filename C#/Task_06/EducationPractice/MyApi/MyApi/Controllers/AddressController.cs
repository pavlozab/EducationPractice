using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dto;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;

namespace MyApi.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [ApiController]
    [Route("api/v1/addresses")]
    public class ProductController : ControllerBase
    {
        private readonly IAddressService _service;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IAddressService service, ILogger<ProductController> logger)
        {
            _service = service;
            _logger = logger;
        }
        
        /// <summary>
        /// Get Products.
        /// </summary>
        /// <response code="200">Returns Product List.</response>
        // [AllowAnonymous]
        // [HttpGet]
        // [ProducesResponseType(200)]
        // public async Task<ActionResult<PagedResponse<Product>>> GetProducts([FromQuery]QueryParametersModel filter)
        // {
        //     _logger.LogInformation("Returned all products");
        //     var products = await _service.GetAll(filter);
        //     var count = await _service.Count();
        //     return Ok(new PagedResponse<Product>(products, count));
        // }
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<Address>> GetProducts()
        {
            _logger.LogInformation("Returned all products");
            var products = await _service.GetAll();
            var count = await _service.Count();
            return Ok(products);
        }

        /// <summary>
        /// Get a specific Product.
        /// </summary>
        /// <param name="id">The id of the item to be retrieved.</param>
        /// <response code="200">Returns a specific Product.</response>
        /// <response code="404">Product hasn't been found.</response>
        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Address>> GetProduct(Guid id)
        {
            var product = await _service.GetOne(id);

            if (product is null)
                return NotFound("Product hasn't been found.");

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
        /// <response code="201">Returns the newly created item.</response>
        /// <response code="400">Product is not created.</response>  
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Address>> CreateProduct(CreateProductDto productDto)
        {
            var product = await _service.Create(productDto);
                
            _logger.LogInformation("Create a Product");
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        /// <summary>
        /// Update a specific Product.
        /// </summary>
        /// <remarks>
        ///  Sample request:
        ///
        ///     PUT /products
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
        /// <param name="id">The id of the item to be updated.</param>
        /// <param name="productDto">Updated product.</param>
        /// <response code="204">Updated product.</response>
        /// <response code="404">Product hasn't been found.</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateProduct(Guid id, UpdateProductDto productDto)
        {
            try
            {
                await _service.Update(id, productDto);

                _logger.LogInformation("Updated product with id: {0}", id);
                return NoContent();
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Deletes a specific Product.
        /// </summary>
        /// <param name="id">The id of the item to be deleted</param>
        /// <response code="204">Deleted product</response>
        /// <response code="404">Product hasn't been found.</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            try
            {
                await _service.Delete(id);

                _logger.LogInformation("Deleted product with id: {0}", id);
                return NoContent();
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}