using System;
using System.ComponentModel.DataAnnotations;

namespace Dto
{
    public class CreateOrderDto
    {
        [Required]
        public Guid AddressId { get; set; }
        
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int Amount { get; set; }
    }
}