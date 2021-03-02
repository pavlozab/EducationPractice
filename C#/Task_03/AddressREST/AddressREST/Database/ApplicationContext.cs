using AddressREST.Models;
using Microsoft.EntityFrameworkCore;

namespace AddressREST.Database
{
    public class ApplicationContext : DbContext
    {
        public DbSet<AddressProduct> Addresses { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options): base(options)
        {
            Database.EnsureCreated();
        }
    }
}