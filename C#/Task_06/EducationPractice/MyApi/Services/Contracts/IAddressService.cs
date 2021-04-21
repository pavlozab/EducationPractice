using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Dto;
using Dto;
using Entities;

namespace Services
{
    public interface IAddressService
    {
        Task<PaginatedResponseDto<Address>> GetAll(QueryMetaDto queryMetaDto);
        Task<Address> GetOne(Guid id);
        Task<Address> Create(CreateProductDto productDto, Guid UserId);
        Task Update(Guid id, UpdateProductDto productDto, Guid UserId);
        Task Delete(Guid id, Guid UserId);
        Task<long> Count();
    }
}