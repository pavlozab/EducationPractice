using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductRest.Dtos;
using ProductRest.Filters;

namespace ProductRest.Repositories
{
    public interface IProductsRepository
    {
        Task<ProductDto> GetProductAsync(Guid id);    
        
        Task<IEnumerable<ProductDto>> GetProductsAsync(PaginationFilter filter);

        Task CreateProductAsync(ProductDto item);

        Task UpdateProductAsync(ProductDto item);

        Task DeleteProductAsync(Guid id);
    }
}