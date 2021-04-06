using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductRest.Data.Contracts;
using ProductRest.Dtos;
using ProductRest.Models;
using ProductRest.Responses;

namespace ProductRest.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/v1/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductsRepository _repository;
        private readonly ILogger<ProductController> _logger;
        private readonly IMapper _mapper;

        public ProductController(IProductsRepository repository, ILogger<ProductController> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        
        /// <summary>
        /// Get Products.
        /// </summary>
        /// <response code="200">Returns Product List</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<PagedResponse<ProductDto>>> GetProducts([FromQuery]QueryParametersModel filter)
        {
            var validFilter = new QueryParametersModel(filter);
            var products = await _repository.GetProductsAsync(validFilter);
            var count = await _repository.Count();
                
            _logger.LogInformation("Returned all products");
            return Ok(new PagedResponse<ProductDto>(products, validFilter, count));
        }

        /// <summary>
        /// Get a specific Product.
        /// </summary>
        /// <param name="id">The id of the item to be retrieved</param>
        /// <response code="200">Returns a specific Product</response>
        /// <response code="404">Product hasn't been found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ProductDto>> GetProduct(Guid id)
        {
            var product = await _repository.GetProductAsync(id);

            if (product is null)
            {
                _logger.LogInformation("Product with id: {0}, hasn't been found.", id);
                return NotFound();
            }
                
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
        ///        "faxNumber": "+380111111111"
        ///        "phoneNumber": "+380222222222"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">One or more validation errors occurred.</response>    
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto productDto)
        {
            ProductDto product = new()
            {
                Id = Guid.NewGuid(),
                AddressLine = productDto.AddressLine,
                PostalCode = productDto.PostalCode,
                Country = productDto.Country,
                City = productDto.City,
                FaxNumber = productDto.FaxNumber,
                PhoneNumber = productDto.PhoneNumber
            };
            await _repository.CreateProductAsync(product);

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
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateProduct(Guid id, CreateProductDto productDto)
        {
            var existingProduct = await  _repository.GetProductAsync(id);

            if (existingProduct is null)
            {
                _logger.LogInformation("Product with id: {0}, hasn't been found.", id);
                return NotFound();
            }

            _mapper.Map(productDto, existingProduct);
                
            await _repository.UpdateProductAsync(existingProduct);

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
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            var existingProduct = await _repository.GetProductAsync(id);

            if (existingProduct is null)
            {
                _logger.LogInformation("Product with id: {0}, hasn't been found.", id);
                return NotFound();
            }

            await _repository.DeleteProductAsync(id);
                
            _logger.LogInformation("Deleted product with id: {0}", id);
            return NoContent();
        }
    }
}