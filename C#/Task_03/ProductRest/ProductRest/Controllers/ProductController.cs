using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductRest.Dtos;
using ProductRest.Models;
using ProductRest.Repositories;
using ProductRest.Responses;

namespace ProductRest.Controllers
{
    [ApiController]
    [Route("api/v1/products")]
    public class ProductController : ControllerBase
    {
        private readonly PostgresqlDbProductRepository _repository;
        private readonly ILogger<ProductController> _logger;
        private readonly IMapper _mapper;

        public ProductController(PostgresqlDbProductRepository repository, ILogger<ProductController> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        
        // GET /products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts([FromQuery]QueryParametersModel filter)
        {
            _logger.LogInformation("Returned all products.");
            try
            {
                var validFilter = new QueryParametersModel(filter.SortBy, filter.SortType, filter.Offset, filter.Limit);
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

        // GET /products/{id}
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

        // POST /products
        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto productDto)
        {
            try
            {
                Console.Write("ad");
                var product = _mapper.Map<ProductDto>(productDto);

                await _repository.CreateProductAsync(product);

                _logger.LogInformation("Created product.");
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product); 
            }
            catch (Exception e)
            {
                _logger.LogError("Something went wrong inside CreateProduct action: {0}", e.Message); 
                return StatusCode(500, "Internal server error"); 
            }
            
        }

        // PUT /products/{id}
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

                ProductDto updatedProduct = existingProduct with
                {
                    AddressLine = productDto.AddressLine,
                    PostalCode = productDto.PostalCode,
                    Country = productDto.Country,
                    City = productDto.City,
                    FaxNumber = productDto.FaxNumber,
                    PhoneNumber = productDto.PhoneNumber
                };
                
                _repository.UpdateProductAsync(updatedProduct);

                _logger.LogInformation("Updated product with id: {0}", id);
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError("Something went wrong inside CreateProduct action: {0}", e.Message); 
                return StatusCode(500, "Internal server error"); 
            }
            
        }

        // DELETE /products/{id}
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

                var product = await _repository.GetProductAsync(id);
                
                _repository.DeleteProductAsync(product);
                
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