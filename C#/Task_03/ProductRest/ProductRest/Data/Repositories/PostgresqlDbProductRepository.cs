using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductRest.Data.Context;
using ProductRest.Dtos;
using ProductRest.Models;

namespace ProductRest.Data.Repositories
{
    public class PostgresqlDbProductRepository : IProductsRepository
    {
        private ProductContext ProductContext { get; set; }
        public DbSet<ProductDto> Products { get; }

        public PostgresqlDbProductRepository(ProductContext context)
        {
            ProductContext = context;
            Products = ProductContext.Set<ProductDto>();
        }

        public async Task<ProductDto> GetProductAsync(Guid id)
        {
            return await Products.Where(obj => obj.Id.Equals(id))
            .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync(QueryParametersModel filter)
        {
            return await Products.AsNoTracking()
                .OrderBy(obj => obj.AddressLine)
                .ToListAsync();
        }

        public async Task CreateProductAsync(ProductDto item)
        {
            await ProductContext.Set<ProductDto>()
                .AddAsync(item);
        }

        public async Task UpdateProductAsync(ProductDto item)
        {
            Products.Update(item);
            await ProductContext.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var product = await Products.Where(obj => obj.Id.Equals(id))
                .FirstOrDefaultAsync();
            Products.Update(product);
            await ProductContext.SaveChangesAsync();
        }

        public async Task<long> Count()
        {
            return await ProductContext.Set<ProductDto>().CountAsync();
        }
    }
}