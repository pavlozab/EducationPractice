using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Task01
{
    /// <summary>
    /// Class for Collection on list representation.
    /// Contain all method for performing some list functions.
    /// </summary>
    /// <remarks>This class can read-write json, search, sort, delete, add, edit object.</remarks>
    public class MyCollection
    {
        private List<Address> _data;

        /// <summary>Initializes a new "MyCollection" object.</summary>
        public MyCollection()
        {
            this._data = new List<Address>();
        }

        /// <summary> Returns a String which represents the object instance.</summary>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this._data, Formatting.Indented);
        }

        /// <summary>Validate Address object.</summary>
        /// <param name="newJsonAddress">Address object</param>
        /// <returns>True if object is valid.</returns>
        /// <exception cref="ValidationException">
        /// Thrown string with all error message when one or more Address parameters is invalid. 
        /// </exception>
        static bool ValidateObject(Address newJsonAddress)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(newJsonAddress);

            if (!Validator.TryValidateObject(newJsonAddress, context, results, true))
            {
                throw new ValidationException(results.Aggregate("",
                    (current, error) => current + (error.ErrorMessage) + 
                                        "\nYour value: " + 
                                        typeof(Address).GetProperty(error.ErrorMessage.Split(' ')[0]).
                                            GetValue(newJsonAddress, null).ToString()+ "\n") 
                );
            }

            return true;
        }

        /// <summary>Read json file and added them to collection.</summary>
        /// <param name="filePath">String representation of file name which contained in "resources" folder.</param>
        public void ReadJson(string fileName = "data.json")
        {
            this._data = new List<Address>();

            using (var r = new StreamReader(@"../resources/" + fileName))
            {
                foreach (var i in JsonConvert.DeserializeObject<List<Address>>(r.ReadToEnd()))
                {
                    try
                    {
                        if (ValidateObject(i)) this._data.Add(i);
                    }
                    catch (ValidationException e)
                    {
                        Console.WriteLine("\nValidation error:\n");
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }

        /// <summary>Write Address objects from collection to json file.</summary>
        /// <param name="filePath">String representation of file name which contained in "resources" folder.</param>
        public void WriteInFile(string fileName = "data.json")
        {
            using (StreamWriter sw = new StreamWriter(@"../resources/" + fileName, 
                false, System.Text.Encoding.Default))
            {
                sw.Write(this.ToString());
            }
        }

        /// <summary> Search in collection of Address objects by string representation of specified value.</summary>
        /// <param name="searchValue">String representation of search value.</param>
        /// <returns>List of objects with the found value</returns>
        public List<Address> Search(string searchValue)
        {
            return this._data.Where(obj => typeof(Address).GetProperties()
                    .Select(attr => typeof(Address).GetProperty(attr.Name).GetValue(obj, null))
                    .Any(temp => temp.ToString().ToLower().Contains(searchValue)))
                .ToList();
        }

        /// <summary>Check property.</summary>
        /// <param name="property">String representation of Address property.</param>
        /// <param name="hideParam">If property == one of this param, return false</param>
        /// <returns>True if property is valid.</returns>
        /// <exception cref="ArgumentException">Invalid parameter</exception>
        private static bool CheckProperty(string property, string[] hideParam = null)
        {
            if (!typeof(Address).GetProperties().All(obj => property != obj.Name) & 
                (hideParam ?? Array.Empty<string>()).All(obj=>obj != property)) return true;
            throw new ArgumentException($"\"{property}\" is invalid.");
        }
        
        /// <summary>Check id.</summary>
        /// <param name="objId">String representation of object id which should be checked.</param>
        /// <returns>True if id is valid.</returns>
        /// <exception cref="ArgumentException">Invalid id</exception>
        private bool CheckId(int objId)
        {
            if (this._data.Any(obj => obj.Id == objId)) return true;
            throw new ArgumentException($"\"{objId}\" id is invalid.");
        }
    
        /// <summary>Sort collection by string representation of address property.</summary>
        /// <param name="sortBy">String representation of Address property. Sorted by this param.</param>
        public void Sort(string sortBy)
        {
            if (!CheckProperty(sortBy)) return;
            
            var properties = typeof(Address).GetProperty(sortBy);
                
            this._data.Sort((address, address1) => string.Compare(
                properties.GetValue(address, null).ToString().ToLower(),
                properties.GetValue(address1, null).ToString().ToLower(),
                StringComparison.Ordinal));
        }

        /// <summary>Delete address object from collection by id.</summary>
        /// <param name="id">Id of address object which must be removed.</param>
        /// <exception cref="Exception"></exception>
        public void Delete(int id)
        {
            if (!_data.Remove(this._data.Find(obj => obj.Id == id)))
            {
                throw new Exception("No address with such ID found");
            }
        }
    
        /// <summary>Add new Address object to collection.</summary>
        public void AddNewObj()
        {
            var newObj = new Address(_data.Max(obj => obj.Id)+1);
        
            foreach (var attr in typeof(Address).GetProperties().Skip(1))
            {
                Console.Write("Enter {0}: ", attr.Name);
                var strValue = Console.ReadLine();
                var properties = typeof(Address).GetProperty(attr.Name);
                
                properties.SetValue(newObj, Convert.ChangeType(strValue, properties.PropertyType), null);
            }
            if (ValidateObject(newObj)) { _data.Add(newObj); }
        }

        /// <summary>Edit object.</summary>
        /// <param name="objId">Object id which must be edited.</param>
        /// <param name="param">Object property which must be edited.</param>
        /// <param name="value">New object value</param>
        /// <exception cref="ArgumentException"></exception>
        public void EditObject(int objId, string param, string value)
        {
            var hiddenProperty = new[] {"Id"};
            
            if (CheckProperty(param, hiddenProperty) & CheckId(objId))
            {
                var properties = typeof(Address).GetProperty(param);
                var currentObj = this._data.Find(obj => obj.Id == objId);
                var currentValue = properties.GetValue(currentObj);
                
                properties.SetValue(currentObj, Convert.ChangeType(value, properties.PropertyType), null);
                try
                {
                    ValidateObject(currentObj);
                }
                catch (ValidationException e)
                {
                    Console.WriteLine("\nValidation error:\n");
                    Console.WriteLine(e.Message);
                    properties.SetValue(currentObj, currentValue, null);
                }
            }
            else { throw new ArgumentException("Invalid data!"); }
        }
    }
}


