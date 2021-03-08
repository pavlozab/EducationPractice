using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductRest.Dtos;
using ProductRest.Models;

namespace ProductRest.Repositories
{
    public class PostgresqlDbProductRepository //: IProductsRepository
    {
        private ProductContext ProductContext { get; set; }

        public PostgresqlDbProductRepository(ProductContext context)
        {
            ProductContext = context;
        }

        public async Task<ProductDto> GetProductAsync(Guid id)
        {
            return await ProductContext.Set<ProductDto>()
                .Where(obj => obj.Id.Equals(id))
                .FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync(QueryParametersModel filter)
        {
            return await ProductContext.Set<ProductDto>().AsNoTracking()
                .OrderBy(obj => obj.AddressLine)
                .ToListAsync();
        }

        public async Task CreateProductAsync(ProductDto item)
        {
            await ProductContext.Set<ProductDto>()
                .AddAsync(item);
        }

        public void UpdateProductAsync(ProductDto item)
        {
            //var filter = _filterDefinitionBuilder.Eq(existingItem => existingItem.Id, item.Id);
            ProductContext.Set<ProductDto>().Update(item);
        }

        public void DeleteProductAsync(ProductDto obj)
        {
            ProductContext.Set<ProductDto>().Remove(obj);
        }

        public async Task<int> Count()
        {
            return await ProductContext.Set<ProductDto>().CountAsync();
        }
    }
}