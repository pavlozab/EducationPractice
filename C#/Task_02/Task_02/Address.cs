using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Task_02
{
    /// <summary> Class for Address representation. </summary>
    public class Address : BaseProduct
    {
        [RegularExpression(@"[a-zA-z,.\s]+", ErrorMessage = "{0} must contain only letters ")]
        public string AddressLine { get; set; }
        
        [StringLength(5, ErrorMessage = "{0} length must be {1}.", MinimumLength = 5)]
        [RegularExpression(@"\d+", ErrorMessage = "{0} must contain only number")]
        public string PostalCode { get; set; }
        
        [RegularExpression(@"[a-zA-Z,.\s]+", ErrorMessage = "{0} must contain only letters")]
        public string Country { get; set; }
        
        [RegularExpression(@"[a-zA-Z,.\s]+", ErrorMessage = "{0} must contain only letters")]
        public string City { get; set; }

        [Phone]
        [StringLength(13, ErrorMessage = "{0} length must be {1}.", MinimumLength = 13)]
        public string FaxNumber { get; set; }

        [Phone]
        [StringLength(13, ErrorMessage = "{0} length must be {1}.", MinimumLength = 13)]
        public string PhoneNumber { get; set; }

        /// <summary>Returns a String which represents the object instance.</summary>
        public override string ToString()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            return JsonSerializer.Serialize<Address>(this, options);
        }
    }
}
