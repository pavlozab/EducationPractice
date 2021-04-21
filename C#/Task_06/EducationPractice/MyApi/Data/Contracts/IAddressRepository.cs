using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Dto;
using Entities;

namespace Data
{
    public interface IAddressRepository : IBaseRepository<Address>
    {
        Task<PaginatedResponseDto<Address>> GetAll(QueryMetaDto queryMetaDto); 
        Task Update(Address item);
        Task<long> Count();
    }
}