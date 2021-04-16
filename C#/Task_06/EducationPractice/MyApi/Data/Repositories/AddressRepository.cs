using System.Collections.Generic;
using System.Threading.Tasks;
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

        // public async Task<IEnumerable<Address>> GetAll(QueryParametersModel filter)
        // {
        //     throw new NotImplementedException();
        // }
        
        public async Task<IEnumerable<Address>> GetAll()
        {
            return await _context.Addresses.ToListAsync();
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