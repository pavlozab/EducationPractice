using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductRest.Dtos;
using ProductRest.Repositories;

namespace ProductRest.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductsRepository _repository;
        private readonly ILogger<ProductController> _logger;
        
        public ProductController(IProductsRepository repository, ILogger<ProductController> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        
        // GET /products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            try
            {
                var items = (await _repository.GetProductsAsync())
                    .Select(item => item.AsDto());
                
                _logger.LogInformation("Returned all products.");
                return Ok(items);
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
                var item = await _repository.GetProductAsync(id);

                if (item is null)
                {
                    _logger.LogInformation("Product with id: {0}, hasn't been found.", id);
                    return NotFound();
                }
                
                _logger .LogInformation("Returned product with id: {0}", id);
                return Ok(item.AsDto());
            }
            catch (Exception e)
            {
                _logger.LogError("Something went wrong inside GetProduct action: {0}", e.Message); 
                return StatusCode(500, "Internal server error"); 
            }
        }

        // POST /products
        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto itemDto)
        {
            try
            {
                ProductDto item = new()
                {
                    Id = Guid.NewGuid(),
                    AddressLine = itemDto.AddressLine,
                    PostalCode = itemDto.PostalCode,
                    Country = itemDto.Country,
                    City = itemDto.City,
                    FaxNumber = itemDto.FaxNumber,
                    PhoneNumber = itemDto.PhoneNumber
                };
            
                await _repository.CreateProductAsync(item);

                _logger.LogInformation("Created product.");
                return CreatedAtAction(nameof(GetProduct), new { id = item.Id }, item.AsDto()); 
            }
            catch (Exception e)
            {
                _logger.LogError("Something went wrong inside CreateProduct action: {0}", e.Message); 
                return StatusCode(500, "Internal server error"); 
            }
            
        }

        // PUT /products/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(Guid id, CreateProductDto itemDto)
        {
            try
            {
                var existingItem = await  _repository.GetProductAsync(id);

                if (existingItem is null)
                {
                    _logger.LogInformation("Product with id: {0}, hasn't been found.", id);
                    return NotFound();
                }

                ProductDto updatedItem = existingItem with
                {
                    AddressLine = itemDto.AddressLine,
                    PostalCode = itemDto.PostalCode,
                    Country = itemDto.Country,
                    City = itemDto.City,
                    FaxNumber = itemDto.FaxNumber,
                    PhoneNumber = itemDto.PhoneNumber
                };
            
                await _repository.UpdateProductAsync(updatedItem);

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
                var existingItem = await _repository.GetProductAsync(id);

                if (existingItem is null)
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