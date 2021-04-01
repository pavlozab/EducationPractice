using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductRest.Entities;
using ProductRest.Models;

namespace ProductRest.Repositories.Contracts
{
    public interface IProductsRepository
    {
        Task<Product> GetProductAsync(Guid id);
        Task<IEnumerable<Product>> GetProductsAsync(QueryParametersModel filter);
        Task CreateProductAsync(Product item);
        Task UpdateProductAsync(Product item);
        Task DeleteProductAsync(Guid id);
        Task<long> Count();
    }
}