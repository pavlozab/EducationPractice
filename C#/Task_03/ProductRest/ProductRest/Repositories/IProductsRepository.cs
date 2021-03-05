using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductRest.Dtos;

namespace ProductRest.Repositories
{
    public interface IProductsRepository
    {
        Task<ProductDto> GetProductAsync(Guid id);    
        
        Task<IEnumerable<ProductDto>> GetProductsAsync();

        Task CreateProductAsync(ProductDto item);

        Task UpdateProductAsync(ProductDto item);

        Task DeleteProductAsync(Guid id);
    }
}