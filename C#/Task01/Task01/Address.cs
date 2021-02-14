using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Task01
{
    /// <summary> Class for Address representation. </summary>
    public class Address
    {
        public int Id { get; init; } 
        
        [Required]
        [RegularExpression(@"[a-zA-z,.\s]+", ErrorMessage = "{0} must contain only letters ")]
        public string AddressLine { get; set; }

        [Required]
        [StringLength(5, ErrorMessage = "{0} length must be {1}.", MinimumLength = 5)]
        [RegularExpression(@"\d+", ErrorMessage = "{0} must contain only number")]
        public string PostalCode { get; set; }
        
        [Required]
        [RegularExpression(@"[a-zA-Z,.\s]+", ErrorMessage = "{0} must contain only letters")]
        public string Country { get; set; }
        
        [Required]
        [RegularExpression(@"[a-zA-Z,.\s]+", ErrorMessage = "{0} must contain only letters")]
        public string City { get; set; }

        [Phone]
        [Required]
        [StringLength(13, ErrorMessage = "{0} length must be {1}.", MinimumLength = 13)]
        public string FaxNumber { get; set; }

        [Phone] 
        [Required]
        [StringLength(13, ErrorMessage = "{0} length must be {1}.", MinimumLength = 13)]
        public string PhoneNumber { get; set; }
        
        /// <summary> Constructor for json serializer. </summary>
        [JsonConstructor]
        public Address() { }
    
        /// <summary>Initializes a new Address object with one parameter id.</summary>
        /// <param name="id">Id of new object.</param>
        public Address(int id) { this.Id = id; }

        /// <summary>Returns a String which represents the object instance.</summary>
        public override string ToString()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            return JsonSerializer.Serialize<Address>(this, options);
        }
    }
}

