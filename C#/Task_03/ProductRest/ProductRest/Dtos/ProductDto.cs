using System;

namespace ProductRest.Dtos
{
    public record ProductDto // : Product
    {
        public Guid Id { get; init; }
        
        public string AddressLine { get; init; }
        
        public string PostalCode { get; init; }
        
        public string Country { get; init; }
        
        public string City { get; init; }
        
        public string FaxNumber { get; init; }
        
        public string PhoneNumber { get; init; }
    }
}