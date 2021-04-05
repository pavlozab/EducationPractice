using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductRest.Dto.Product;
using ProductRest.Entities;
using ProductRest.Models;

namespace ProductRest.Services.Contracts
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAll(QueryParametersModel filter);
        Task<Product> GetOne(Guid id);
        Task<Product> Create(CreateProductDto productDto);
        Task Update(Guid id, UpdateProductDto productDto);
        Task Delete(Guid id);
        Task<long> Count();
    }
}