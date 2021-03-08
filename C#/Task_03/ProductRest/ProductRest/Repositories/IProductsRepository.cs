using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductRest.Dtos;
using ProductRest.Models;

namespace ProductRest.Repositories
{
    public interface IProductsRepository
    {
        Task<ProductDto> GetProductAsync(Guid id);    
        
        Task<IEnumerable<ProductDto>> GetProductsAsync(QueryParametersModel filter);

        Task CreateProductAsync(ProductDto item);

        Task UpdateProductAsync(ProductDto item);

        Task DeleteProductAsync(Guid id);
        
        Task<long> Count();
    }
}