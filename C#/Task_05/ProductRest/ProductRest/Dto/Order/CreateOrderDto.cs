using System;
using System.ComponentModel.DataAnnotations;

namespace ProductRest.Dto.Order
{
    public class CreateOrderDto
    {
        [Required]
        public Guid ProductId { get; set; }
        
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int Amount { get; set; }
    }
}