using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities
{
    public class Address : BaseEntity
    {
        public string AddressLine { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string FaxNumber { get; set; }
        public string PhoneNumber { get; set; }
        public int Amount { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
    
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.Property(p => p.Amount).IsRequired();
        }
    }
}