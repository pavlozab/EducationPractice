using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ProductRest.Dto;
using ProductRest.Dto.Product;
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

        public async Task<IEnumerable<Product>> GetAll(QueryParametersModel filter)
        {
            var validFilter = new QueryParametersModel(filter);
            var products = await _repository.GetAll(validFilter);
            return products;
        }

        public async Task<Product> GetOne(Guid id)
        {
            var product = await  _repository.Get(id);
            
            if (product is null)
                throw new KeyNotFoundException("Product hasn't been found");
            
            return product;
        }

        public async Task<Product> Create(CreateProductDto productDto)
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
            await _repository.Create(product);
            return product;
        }

        public async Task Update(Guid id, UpdateProductDto productDto)
        {
            var existingProduct = await  _repository.Get(id);

            if (existingProduct is null)
                throw new KeyNotFoundException("Product hasn't been found");
            
            _mapper.Map(productDto, existingProduct);

            await _repository.Update(existingProduct);
        }

        public async Task Delete(Guid id)
        {
            var existingProduct = await _repository.Get(id);

            if (existingProduct is null)
                throw new KeyNotFoundException("Product hasn't been found");

            await _repository.Delete(id);
        }

        public async Task<long> Count()
        {
            return await _repository.Count();
        }
    }
}