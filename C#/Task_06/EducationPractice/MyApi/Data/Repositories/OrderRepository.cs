using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        // public OrderRepository(ApplicationDbContext context)
        //     : base(context) { }

        public async Task<IEnumerable<Order>> GetAll(Guid userId)
        {
            return await _context.Orders.Where(obj => obj.UserId == userId).ToListAsync();
        }
    }
}