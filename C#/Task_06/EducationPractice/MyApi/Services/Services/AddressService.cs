using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Data;
using Data.Dto;
using Entities;
using Dto;
using Services.Cache;

namespace Services
{
    public class AddressService: IAddressService
    {
        private readonly IAddressRepository _repository;
        private readonly ICacheClient _cacheClient;
        private readonly IMapper _mapper;

        public AddressService(IAddressRepository repository, IMapper mapper, ICacheClient cacheClient)
        {
            _repository = repository;
            _mapper = mapper;
            _cacheClient = cacheClient;
        }

        public async Task<IEnumerable<AddressResponseDto>> GetAll(QueryMetaDto queryMetaDto)
        {
            IEnumerable<AddressResponseDto> result;

            if (queryMetaDto.Search is null)
            {
                result = await _cacheClient.GetQuery(queryMetaDto);
                
                if (result is null)
                {
                    var addresses = await _repository.GetAll(queryMetaDto);
                    result = _mapper.Map<IEnumerable<Address>, IEnumerable<AddressResponseDto>>(addresses);
                    await _cacheClient.SetQuery(queryMetaDto, result);
                }
            }
            else
            {
                var addresses = await _repository.GetAll(queryMetaDto);
                result = _mapper.Map<IEnumerable<Address>, IEnumerable<AddressResponseDto>>(addresses);
            }

            return result;
        }

        public async Task<AddressResponseDto> GetOne(Guid id)
        {
            var address = await _repository.GetOne(id);
            return _mapper.Map<AddressResponseDto>(address);
        }

        public async Task<AddressResponseDto> Create(CreateAddressDto addressDto)
        {
            var product = _mapper.Map<Address>(addressDto);
            await _repository.Create(product);
            
            _cacheClient.RemoveAllCache();

            return _mapper.Map<AddressResponseDto>(product);
        }

        public async Task Update(Guid id, UpdateAddressDto addressDto)
        {
            var existingProduct = await  _repository.GetOne(id);

            if (existingProduct is null)
                throw new KeyNotFoundException("Address hasn't been found");
            
            _mapper.Map(addressDto, existingProduct);

            _cacheClient.RemoveAllCache();
            await _repository.Update(existingProduct);
        }

        public async Task Delete(Guid id)
        {
            var existingProduct = await _repository.GetOne(id);

            if (existingProduct is null)
                throw new KeyNotFoundException("Address hasn't been found");

            await _repository.Delete(existingProduct);
            _cacheClient.RemoveAllCache();
        }

        public async Task<long> Count()
        {
            return await _repository.Count();
        }
        
        // private async Task<IEnumerable<Address>> GetFromCache(QueryMetaDto queryMetaDto)
        // {
        //     
        // }

        // private static void GenerateNewCache(object sender, EventArgs eventArgs)
        // {
        // }
    }
}