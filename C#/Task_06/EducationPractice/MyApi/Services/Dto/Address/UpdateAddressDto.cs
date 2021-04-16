using System.ComponentModel.DataAnnotations;

namespace Dto
{
    public class UpdateProductDto: CreateProductDto
    {
        // [Required]
        // [RegularExpression(@"[a-zA-z,.\s]+", ErrorMessage = "{0} must contain only letters ")]
        // public string AddressLine { get; set; }
        //
        // [Required]
        // [StringLength(5, ErrorMessage = "{0} length must be {1}.", MinimumLength = 5)]
        // [RegularExpression(@"\d+", ErrorMessage = "{0} must contain only number")]
        // public string PostalCode { get; set; }
        //
        // [Required]
        // [RegularExpression(@"[a-zA-Z,.\s]+", ErrorMessage = "{0} must contain only letters")]
        // public string Country { get; set; }
        //
        // [Required]
        // [RegularExpression(@"[a-zA-Z,.\s]+", ErrorMessage = "{0} must contain only letters")]
        // public string City { get; set; }
        //
        // [Required]
        // [Phone]
        // [StringLength(13, ErrorMessage = "{0} length must be {1}.", MinimumLength = 13)]
        // public string FaxNumber { get; set; }
        //
        // [Required]
        // [Phone]
        // [StringLength(13, ErrorMessage = "{0} length must be {1}.", MinimumLength = 13)]
        // public string PhoneNumber { get; set; }
        //
        // [Required]
        // [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        // public int Amount { get; set; }
    }
}