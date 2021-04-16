using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dto;
using Entities;

namespace Services
{
    public interface IAddressService
    {
        Task<IEnumerable<Address>> GetAll(); //(QueryParametersModel filter);
        Task<Address> GetOne(Guid id);
        Task<Address> Create(CreateProductDto productDto);
        Task Update(Guid id, UpdateProductDto productDto);
        Task Delete(Guid id);
        Task<long> Count();
    }
}