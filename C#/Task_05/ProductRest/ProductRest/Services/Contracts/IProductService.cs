using System;
using System.Threading.Tasks;
using ProductRest.Dto;
using ProductRest.Entities;
using ProductRest.Models;
using ProductRest.Responses;

namespace ProductRest.Services.Contracts
{
    public interface IProductService
    {
        Task<PagedResponse<Product>> GetProducts(QueryParametersModel filter);
        Task<Product> GetProduct(Guid id);
        Task<Product> CreateProduct(CreateProductDto productDto);
        Task<Product> UpdateProduct(Guid id, UpdateProductDto productDto);
        Task<Product> DeleteProduct(Guid id);
    }
}