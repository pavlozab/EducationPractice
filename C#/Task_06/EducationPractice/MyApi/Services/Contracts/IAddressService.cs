using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Dto;
using Dto;

namespace Services
{
    public interface IAddressService
    {
        Task<IEnumerable<AddressResponseDto>> GetAll(QueryMetaDto queryMetaDto);
        Task<AddressResponseDto> GetOne(Guid id);
        Task<AddressResponseDto> Create(CreateAddressDto addressDto);
        Task Update(Guid id, UpdateAddressDto addressDto);
        Task Delete(Guid id);
        Task<long> Count();
    }
}