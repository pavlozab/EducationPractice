using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Dto;
using Dto;

namespace Services.Cache
{
    public interface ICacheClient
    {
        Task<IEnumerable<AddressResponseDto>> GetQuery(QueryMetaDto queryMetaDto);
        Task SetQuery(QueryMetaDto queryMetaDto, IEnumerable<AddressResponseDto> addresses);
        void RemoveAllCache();
    }
}