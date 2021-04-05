using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductRest.Entities
{
    public class Product 
    {
        public Guid Id { get; init; }
        public string AddressLine { get; init; }
        public string PostalCode { get; init; }
        public string Country { get; init; }
        public string City { get; init; }
        public string FaxNumber { get; init; }
        public string PhoneNumber { get; init; }
        public int Amount { get; set; }
    }
}