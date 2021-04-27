using System;

namespace Dto
{
    public class AddressResponseDto //: CreateOrderDto  , BaseDto<,>
    {
        public Guid Id { get; set; }
        public string AddressLine { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string FaxNumber { get; set; }
        public string PhoneNumber { get; set; }
        public int Amount { get; set; }
    }
}