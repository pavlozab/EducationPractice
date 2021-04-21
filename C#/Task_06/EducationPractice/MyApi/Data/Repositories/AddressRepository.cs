using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Dto;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class AddressRepository : BaseRepository<Address>, IAddressRepository
    {
        // public AddressRepository(ApplicationDbContext context)
        //     : base(context)
        // {
        // }

        public async Task<PaginatedResponseDto<Address>> GetAll(QueryMetaDto queryMetaDto)
        {
            var strReprOfSearch = "";
            // return await _context.Addresses.Find() Where(obj =>
            //     EF.Functions.ILike(obj.AddressLine, $"%{queryMetaDto.Search}%"))
            //     .Skip(queryMetaDto.Offset * queryMetaDto.Limit)
            //     .Take(queryMetaDto.Limit)
            //     .ToListAsync();
            
            // var addresses =  await _context.Addresses.Where(obj
            //     => obj.AddressLine.Contains(queryMetaDto.Search)
            //        || obj.PostalCode.Contains(queryMetaDto.Search)
            //        || obj.Country.Contains(queryMetaDto.Search)
            //        || obj.City.Contains(queryMetaDto.Search)
            //        || obj.FaxNumber.Contains(queryMetaDto.Search)
            //        || obj.PhoneNumber.Contains(queryMetaDto.Search)
            // ).Skip(queryMetaDto.Offset * queryMetaDto.Limit)
            // .Take(queryMetaDto.Limit)
            // .ToListAsync();
            var addresses =  await _context.Addresses.Skip(queryMetaDto.Offset * queryMetaDto.Limit)
                .Take(queryMetaDto.Limit)
                .ToListAsync();

            var count = await Count();

            return new PaginatedResponseDto<Address>
            {
                Items = addresses,
                Meta = new MetaDto(queryMetaDto, count)
            };
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