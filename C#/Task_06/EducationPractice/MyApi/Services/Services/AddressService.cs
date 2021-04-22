using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Data;
using Data.Dto;
using Entities;
using Dto;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Services
{
    public class AddressService: IAddressService
    {
        private readonly IAddressRepository _repository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _distributedCache;

        public AddressService(
            IAddressRepository repository, 
            IMapper mapper,
            IDistributedCache distributedCache)
        {
            _repository = repository;
            _mapper = mapper;
            _distributedCache = distributedCache;
        }

        public async Task<IEnumerable<Address>> GetAll(QueryMetaDto queryMetaDto)
        {
            IEnumerable<Address> addresses;
            queryMetaDto.Validate();

            if (queryMetaDto.Search is null)
            {
                addresses = await GetFromCache(queryMetaDto);
            }
            else
            {
                addresses = await _repository.GetAll(queryMetaDto);
            }

            return addresses;
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
            
            RemoveAllCache();

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
            RemoveAllCache();
        }

        public async Task Delete(Guid id, Guid UserId)
        {
            var existingProduct = await _repository.GetOne(id);

            if (existingProduct is null)
                throw new KeyNotFoundException("Product hasn't been found");

            existingProduct.UserId = UserId;

            await _repository.Delete(id);
            RemoveAllCache();
        }

        public async Task<long> Count()
        {
            return await _repository.Count();
        }
        
        private async Task<IEnumerable<Address>> GetFromCache(QueryMetaDto queryMetaDto)
        {
            var key = JsonConvert.SerializeObject(queryMetaDto);
            var cache = await _distributedCache.GetStringAsync(key);
            
            IEnumerable<Address> addresses;
            
            if (cache is null)
            {
                addresses = await _repository.GetAll(queryMetaDto);

                var productsCache = JsonConvert.SerializeObject(addresses);

                await _distributedCache.SetStringAsync(key, productsCache);
            }
            else
            {
                addresses = JsonConvert.DeserializeObject<List<Address>>(cache);
            }
            
            return addresses;
        }

        private void RemoveAllCache()
        {
        }
        
        // private static void GenerateNewCache(object sender, EventArgs eventArgs)
        // {
        // }
    }
}