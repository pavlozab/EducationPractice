using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Dto;
using Dto;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Services.Cache
{
    public class CacheClient: ICacheClient
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase _database;

        public CacheClient(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _database = connectionMultiplexer.GetDatabase();
        }

        public async Task<IEnumerable<AddressResponseDto>> GetQuery(QueryMetaDto queryMetaDto)
        {
            var key = JsonConvert.SerializeObject(queryMetaDto);
            string cache = await _database.StringGetAsync(key);

            return cache is null ? null : JsonConvert.DeserializeObject<List<AddressResponseDto>>(cache);
        }

        public async Task SetQuery(QueryMetaDto queryMetaDto, IEnumerable<AddressResponseDto> addresses)
        {
            var key = JsonConvert.SerializeObject(queryMetaDto);
            var strAddresses = JsonConvert.SerializeObject(addresses);

            await _database.StringSetAsync(key, strAddresses);
        }
        
        public void RemoveAllCache()
        {
            var endpoints = _connectionMultiplexer.GetEndPoints(true);
            foreach (var endpoint in endpoints)
            {
                var server = _connectionMultiplexer.GetServer(endpoint);
                server.FlushAllDatabases();    
            }
        }
    }
}