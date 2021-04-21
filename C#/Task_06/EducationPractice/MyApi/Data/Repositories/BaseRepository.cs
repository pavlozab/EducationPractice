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

        // protected BaseRepository(ApplicationDbContext postgresDbContext)
        // {
        //     _context = postgresDbContext;
        // }

        protected BaseRepository()
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql("Server=localhost;Port=5432;Database=myapi;Username=pasha;Password=1111")
                .Options; // FIXME Dependency inversion
            _context = new ApplicationDbContext(contextOptions);
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

        public async Task Delete(Guid id)
        {
            _context.Remove(await GetOne(id));
            await _context.SaveChangesAsync();
        }
    }
}