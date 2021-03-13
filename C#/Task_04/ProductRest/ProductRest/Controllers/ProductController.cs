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
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts([FromQuery]QueryParametersModel filter)
        {
            try
            {
                var validFilter = new QueryParametersModel(filter);
                var products = await _repository.GetProductsAsync(validFilter);
                var count = await _repository.Count();
                
                _logger.LogInformation("Returned all products.");
                return Ok( new PagedResponse<ProductDto>(products, validFilter, count));
            }
            catch (Exception e)
            {
                _logger.LogError("Something went wrong inside GetProducts action: {0}", e.Message);
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Get a specific Product.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(Guid id)
        {
            try
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
            catch (Exception e)
            {
                _logger.LogError("Something went wrong inside GetProduct action: {0}", e.Message); 
                return StatusCode(500, "Internal server error"); 
            }
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
        /// <param name="productDto"></param>
        /// <returns>A newly created Product</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">One or more validation errors occurred.</response>    
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto productDto)
        {
            try
            {
                var product = _mapper.Map<ProductDto>(productDto);
                await _repository.CreateProductAsync(product);

                _logger.LogInformation("Create a Product.");
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product); 
            }
            catch (Exception e)
            {
                _logger.LogError("Something went wrong inside CreateProduct action: {0}", e.Message); 
                return StatusCode(500, "Internal server error"); 
            }
            
        }

        /// <summary>
        /// Update a apecifing
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(Guid id, CreateProductDto productDto)
        {
            try
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
            catch (Exception e)
            {
                _logger.LogError("Something went wrong inside CreateProduct action: {0}", e.Message); 
                return StatusCode(500, "Internal server error"); 
            }
            
        }

        /// <summary>
        /// Deletes a specific Product.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="204"></response>
        /// <response code="400">One or more validation errors occurred.</response>  
        /// <response code="404"></response>  
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            try
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
            catch (Exception e)
            {
                _logger.LogError("Something went wrong inside CreateProduct action: {0}", e.Message); 
                return StatusCode(500, "Internal server error"); 
            }
            
        }
    }
}