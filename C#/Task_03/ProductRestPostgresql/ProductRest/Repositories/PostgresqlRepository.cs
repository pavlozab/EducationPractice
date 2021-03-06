using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductRest.Dtos;
using ProductRest.Models;


namespace ProductRest.Repositories
{
    public class PostgresqlRepository
    {
        private ProductContext ProductContext { get; set; }

        public PostgresqlRepository(ProductContext context)
        {
            ProductContext = context;
        }

        public async Task<ProductDto> GetProductAsync(Guid id)
        {
            return await ProductContext.Set<ProductDto>()
                .Where(obj => obj.Id.Equals(id))
                .AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync(QueryParametersModel filter)
        {
            return await ProductContext.Set<ProductDto>().AsNoTracking()
                .OrderBy(obj => obj.AddressLine)
                .ToListAsync();
        }

        public async Task CreateProductAsync(ProductDto item)
        {
            await ProductContext.Set<ProductDto>().AddAsync(item);
        }

        public void UpdateProductAsync(ProductDto item)
        {
            ProductContext.Set<ProductDto>().Update(item);
        }

        public void DeleteProductAsync(ProductDto id)
        {
            ProductContext.Set<ProductDto>().Remove(id);
        }

        public int CountAsync()
        {
            return ProductContext.Set<ProductDto>().Count();
        }
    }
}