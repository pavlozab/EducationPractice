using Microsoft.EntityFrameworkCore;
using ProductRest.Dtos;

namespace ProductRest.Data.Context
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options)
            : base(options)
        {
        }
        
        public DbSet<ProductDto> Products { get; set; }
    }
}