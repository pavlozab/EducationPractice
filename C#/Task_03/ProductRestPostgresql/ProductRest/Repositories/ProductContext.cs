using Microsoft.EntityFrameworkCore;
using ProductRest.Dtos;

namespace ProductRest.Repositories
{
    public class ProductContext : DbContext
    {
        private DbSet<ProductDto> Products { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=localhost;Database=products;Username=pasha;Password=0000");
    }
}