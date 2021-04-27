using System;

namespace Dto
{
    public class OrderResponseDto
    {
        public Guid Id { get; set; }
        public Guid AddressId { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
    }
}