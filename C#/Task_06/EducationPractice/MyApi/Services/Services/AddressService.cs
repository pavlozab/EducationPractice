using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Data;
using Data.Dto;
using Entities;
using Dto;

namespace Services
{
    public class AddressService: IAddressService
    {
        private readonly IAddressRepository _repository;
        private readonly IMapper _mapper;

        public AddressService(IAddressRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // public async Task<IEnumerable<Address>> GetAll(QueryParametersModel filter)
        // {
        //     var validFilter = new QueryParametersModel(filter);
        //     var products = await _repository.GetAll(validFilter);
        //     return products;
        // }

        public async Task<PaginatedResponseDto<Address>> GetAll(QueryMetaDto queryMetaDto)
        {
            var validQuery = new QueryMetaDto(queryMetaDto);
            return await _repository.GetAll(validQuery);
        }

        public async Task<Address> GetOne(Guid id)
        {
            return await _repository.GetOne(id);
        }

        public async Task<Address> Create(CreateProductDto productDto, Guid UserId)
        {
            Address product = new()
            {
                Id = Guid.NewGuid(),
                AddressLine = productDto.AddressLine,
                PostalCode = productDto.PostalCode,
                Country = productDto.Country,
                City = productDto.City,
                FaxNumber = productDto.FaxNumber,
                PhoneNumber = productDto.PhoneNumber,
                Amount = productDto.Amount,
                UserId = UserId
            };
            await _repository.Create(product);
            return product;
        }

        public async Task Update(Guid id, UpdateProductDto productDto, Guid UserId)
        {
            var existingProduct = await  _repository.GetOne(id);

            if (existingProduct is null)
                throw new KeyNotFoundException("Product hasn't been found");
            
            _mapper.Map(productDto, existingProduct);
            existingProduct.UserId = UserId;

            await _repository.Update(existingProduct);
        }

        public async Task Delete(Guid id, Guid UserId)
        {
            var existingProduct = await _repository.GetOne(id);

            if (existingProduct is null)
                throw new KeyNotFoundException("Product hasn't been found");

            existingProduct.UserId = UserId;

            await _repository.Delete(id);
        }

        public async Task<long> Count()
        {
            return await _repository.Count();
        }
    }
}