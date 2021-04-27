using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities
{
    public class Order: BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid AddressId { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }

        public User User { get; set; }
        public Address Address { get; set; }
    }
    
    
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(p => p.Amount).IsRequired();
            builder.HasOne(p => p.User).WithMany(u => u.Orders).HasForeignKey(p => p.UserId);
            builder.HasOne(p => p.Address).WithMany(u => u.Orders).HasForeignKey(p => p.AddressId);
        }
    }
}