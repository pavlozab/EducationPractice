using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Data 
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        protected ApplicationDbContext _context { get; set; }

        protected BaseRepository(ApplicationDbContext postgresDbContext)
        {
            _context = postgresDbContext;
        }

        public async Task<TEntity> GetOne(Guid id)
        {   
            var result = await _context.Set<TEntity>().FirstOrDefaultAsync(obj => obj.Id == id);
            if (result is null)
            {
                throw new KeyNotFoundException($"{typeof(TEntity)} hasn't been found.");
            }

            return result;
        }

        public async Task Create(TEntity obj)
        {
            await _context.AddAsync(obj);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(TEntity item)
        {
            _context.Remove(item);
            await _context.SaveChangesAsync();
        }
        
        // public void ReceiveNotification()  // database notify
        // {
        //     _context.
        // }
    }
}