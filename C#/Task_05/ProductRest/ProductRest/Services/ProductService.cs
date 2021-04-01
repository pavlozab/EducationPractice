using System;
using System.Threading.Tasks;
using AutoMapper;
using ProductRest.Dto;
using ProductRest.Entities;
using ProductRest.Models;
using ProductRest.Repositories.Contracts;
using ProductRest.Responses;
using ProductRest.Services.Contracts;

namespace ProductRest.Services
{
    public class ProductService: IProductService
    {
        private readonly IProductsRepository _repository;
        private readonly IMapper _mapper;

        public ProductService(IProductsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<Product>> GetProducts(QueryParametersModel filter)
        {
            var validFilter = new QueryParametersModel(filter);
            var products = await _repository.GetProductsAsync(validFilter);
            var count = await _repository.Count();
            return new PagedResponse<Product>(products, validFilter, count);
        }

        public async Task<Product> GetProduct(Guid id)
        {
            return await _repository.GetProductAsync(id);
        }

        public async Task<Product> CreateProduct(CreateProductDto productDto)
        {
            Product product = new()
            {
                Id = Guid.NewGuid(),
                AddressLine = productDto.AddressLine,
                PostalCode = productDto.PostalCode,
                Country = productDto.Country,
                City = productDto.City,
                FaxNumber = productDto.FaxNumber,
                PhoneNumber = productDto.PhoneNumber,
                Amount = productDto.Amount
            };
            await _repository.CreateProductAsync(product);
            return product;
        }

        public async Task<Product> UpdateProduct(Guid id, UpdateProductDto productDto)
        {
            var existingProduct = await  _repository.GetProductAsync(id);

            if (existingProduct is null)
                return null;
            
            _mapper.Map(productDto, existingProduct);

            await _repository.UpdateProductAsync(existingProduct);
            return existingProduct;
        }

        public async Task<Product> DeleteProduct(Guid id)
        {
            var existingProduct = await _repository.GetProductAsync(id);

            if (existingProduct is null)
                return null;

            await _repository.DeleteProductAsync(id);
            return existingProduct;
        }
    }
}