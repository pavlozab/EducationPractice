using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Dto;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class AddressRepository : BaseRepository<Address>, IAddressRepository
    {
        public AddressRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Address>> GetAll(QueryMetaDto queryMetaDto)
        {
            var sql = $"SELECT * FROM \"Addresses\" " +
                      $"WHERE \"AddressLine\" LIKE '%{queryMetaDto.Search}%' OR " +
                      $"\"PostalCode\" LIKE '%{queryMetaDto.Search}%' OR " +
                      $"\"Country\" LIKE '%{queryMetaDto.Search}%' OR " +
                      $"\"City\" LIKE '%{queryMetaDto.Search}%' OR " +
                      $"\"FaxNumber\" LIKE '%{queryMetaDto.Search}%' OR " +
                      $"\"PhoneNumber\" LIKE '%{queryMetaDto.Search}%' " +
                      $"ORDER BY \"{queryMetaDto.SortBy}\" {queryMetaDto.SortType} " +
                      $"LIMIT {queryMetaDto.Limit} OFFSET {queryMetaDto.Offset};";
            
            IEnumerable<Address> addresses = await _context.Addresses.FromSqlRaw(sql).ToListAsync();
            
            return addresses;
        }
        
        public async Task Update(Address obj)
        {
            _context.Addresses.Update(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<long> Count()
        {
            return await _context.Addresses.CountAsync();
        }
    }
}