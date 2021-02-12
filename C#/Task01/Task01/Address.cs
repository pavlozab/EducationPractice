using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

public class Address
{
    [JsonConstructor]
    public Address() { }
    
    public Address(int id) { this.Id = id; }

    public int Id { get; init; } 

    [Required]
    [RegularExpression(@"[a-zA-z]+", ErrorMessage = "{0} must contain only letters ")]
    public string AddressLine { get; set; }

    [Required]
    [StringLength(5, ErrorMessage = "{0} length must be {1}.", MinimumLength = 5)]
    [RegularExpression(@"\d+", ErrorMessage = "{0} must contain only number")]
    public string PostalCode { get; set; }
        
    [Required]
    [RegularExpression(@"[a-zA-Z]+", ErrorMessage = "{0} must contain only letters")]
    public string Country { get; set; }
        
    [Required]
    [RegularExpression(@"[a-zA-Z]+", ErrorMessage = "{0} must contain only letters")]
    public string City { get; set; }

    [Phone]
    [Required]
    [StringLength(13, ErrorMessage = "{0} length must be {1}.", MinimumLength = 13)]
    public string FaxNumber { get; set; }

    [Phone] 
    [Required]
    [StringLength(13, ErrorMessage = "{0} length must be {1}.", MinimumLength = 13)]
    public string PhoneNumber { get; set; }
        
        
    // Return the point's value as a string.
    public override string ToString()
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        return JsonSerializer.Serialize<Address>(this, options);
    }
}

