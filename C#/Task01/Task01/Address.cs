using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Task01
{
    /// <summary> Class for Address representation. </summary>
    public class Address
    {
        private int _id;
        private string _addressLine;
        private string _postalCode;
        private string _country;
        private string _city;
        private string _faxNumber;
        private string _phoneNumber;
        
        public int Id { get; init; }

        public string AddressLine
        {
            get => _addressLine;
            set
            {
                if ()
                {
                    _addressLine = value;
                }
            }
        }

        public string PostalCode
        {
            get => _postalCode;
            set
            {
                
            }
        }
        
        public string Country 
        {
            get => _country;
            set
            {
                
            }
        }
        
        public string City 
        {
            get => _city;
            set
            {
                
            }
        }
        public string FaxNumber 
        {
            get => _faxNumber;
            set
            {
                
            }
        }
        public string PhoneNumber 
        {
            get => _phoneNumber;
            set
            {
                
            }
        }
        public Address(int id, string addressLine, 
            string postalCode, string country, 
            string city, string faxNumber, 
            string phoneNumber)
        {
            this._id = id;
            this._addressLine = addressLine;
            this._postalCode = postalCode;
            this._country = country;
            this._city = city;
            this._faxNumber = faxNumber;
            this._phoneNumber = phoneNumber;
        }
        
        /// <summary> Constructor for json serializer. </summary>
        [JsonConstructor]
        public Address() { }
    
        /// <summary>Initializes a new Address object.</summary>
        /// <param name="id">Id of new object.</param>
        public Address(int id) { this._id = id; }

        /// <summary> Returns a String which represents the object instance.</summary>
        public override string ToString()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            return JsonSerializer.Serialize<Address>(this, options);
        }
    }
}

