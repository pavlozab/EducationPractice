using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductRest.Dtos;
using ProductRest.Repositories;

namespace ProductRest.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductsRepository _repository;
        
        public ProductController(IProductsRepository repository)
        {
            _repository = repository;
        }
        
        // GET /products
        [HttpGet]
        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var items = (await _repository.GetProductsAsync())
                                            .Select(item => item.AsDto());
            return items;
        }

        // GET /products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(Guid id)
        {
            var item = await _repository.GetProductAsync(id);

            if (item is null)
                return NotFound();

            return item.AsDto();  // Ok()
        }

        // POST /products
        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto itemDto)
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

            return CreatedAtAction(nameof(GetProduct), new { id = item.Id }, item.AsDto()); 
        }

        // PUT /products/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(Guid id, CreateProductDto itemDto)
        {
            var existingItem = await  _repository.GetProductAsync(id);

            if (existingItem is null)
            {
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

            return NoContent();
        }

        // DELETE /products/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            var existingItem = await _repository.GetProductAsync(id);

            if (existingItem is null)
            {
                return NotFound();
            }

            await _repository.DeleteProductAsync(id);

            return NoContent();
        }
    }
}