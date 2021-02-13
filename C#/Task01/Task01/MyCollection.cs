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
    /// <remarks>
    /// This class can read-write json, search, sort, delete add edit object
    /// </remarks>
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
            return JsonConvert.SerializeObject(_data, Formatting.Indented);
        }

        /// <summary>Validate Address object.</summary>
        /// <param name="newJsonAddress">Address object</param>
        /// <returns>True if object is valid.</returns>
        /// <exception cref="ValidationException">
        /// Thrown string with all error message when one or more Address parameters is invalid. 
        /// </exception>
        static bool ValidateObject(Address newJsonAddress)
        {
            var singleStringErrorMessage = "";
            var results = new List<ValidationResult>();
            var context = new ValidationContext(newJsonAddress);

            if (!Validator.TryValidateObject(newJsonAddress, context, results, true))
            {
                foreach (var error in results)
                {
                    singleStringErrorMessage += error.ErrorMessage + "\n";
                }

                throw new ValidationException(singleStringErrorMessage);
            }
            return true; 
        }
        
        /// <summary>Read json file and added them to collection.</summary>
        /// <param name="filePath">String representation of file path.</param>
        public void ReadJson(string filePath = "resources/data.json") //[FileNameValidator] 
        {
            this._data = new List<Address>();
            var neededPath = System.IO.Directory.GetCurrentDirectory().Replace("bin/Debug/net5.0", 
                filePath);  // :(
            using (var r = new StreamReader(neededPath))
            {
                foreach (var i in JsonConvert.DeserializeObject<List<Address>>(r.ReadToEnd()).Where(
                    i => ValidateObject(i)))
                {
                    _data.Add(i);
                }
            }
        }
        
        /// <summary>Write Address objects from collection to json file.</summary>
        /// <param name="filePath">String representation of file path.</param>
        public void WriteInFile(string filePath = "resources/data.json")
        {
            var writePath = System.IO.Directory.GetCurrentDirectory().Replace("bin/Debug/net5.0", 
                filePath);
            using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
            {
                sw.Write(this.ToString());
            }
        }
    
        /// <summary> Search in collection of Address objects by string representation of specified value.</summary>
        /// <param name="searchValue">String representation of search value.</param>
        /// <returns>List of objects with the found value</returns>
        public List<Address> Search(string searchValue)
        {
            var searchResult = new List<Address>();

            foreach (var obj in this._data)
            {
                foreach (var attr in typeof(Address).GetProperties())
                {
                    var temp = typeof(Address).GetProperty(attr.Name).GetValue(obj, null);
                    if (temp.ToString().ToLower().Contains(searchValue)) { searchResult.Add(obj); break; }
                }
            }
        
            return searchResult;
        }

        /// <summary>Check property.</summary>
        /// <param name="property">String representation of Address property.</param>
        /// <returns>True if property is valid.</returns>
        /// <exception cref="ArgumentException"></exception>
        private static bool CheckProperty(string property)
        {
            if (!typeof(Address).GetProperties().All(obj => property != obj.Name)) return true;
            throw new ArgumentException($"{property} is invalid.");
        }
    
        /// <summary>Sort collection by string representation of address property.</summary>
        /// <param name="sortBy">String representation of Address property. Sorted by this param.</param>
        public void Sort(string sortBy)
        {
            if (CheckProperty(sortBy))
            {
                _data.Sort((address, address1) => string.Compare(
                    typeof(Address).GetProperty(sortBy).GetValue(address, null).ToString().ToLower(),
                    typeof(Address).GetProperty(sortBy).GetValue(address1, null).ToString().ToLower(),
                    StringComparison.Ordinal));
            }
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
                typeof(Address).GetProperty(attr.Name).SetValue(newObj, strValue, null);
            }
            if (ValidateObject(newObj)) { _data.Add(newObj); }
        }

        /// <summary>Edit object.</summary>
        /// <param name="objId">Object id which must be edited.</param>
        /// <param name="param">Object property which must be edited.</param>
        /// <param name="value">New object value</param>
        /// <exception cref="ArgumentException"></exception>
        public void EditObject(int objId, string param, object value)
        {
            if (CheckProperty(param) & !_data.All(obj => obj.Id != objId))
            {
                var results = new List<ValidationResult>();
                var context = new ValidationContext(typeof(Address).GetProperty(param));

                var currentObj = _data.Find(obj => obj.Id == objId);
                var newObj = currentObj;
                
                typeof(Address).GetProperty(param).SetValue(newObj, value, null);
                
                if (ValidateObject(newObj)) { _data[_data.IndexOf(currentObj)] = newObj; }
            }
            else { throw new ArgumentException("Invalid data!"); }
        }
    }
}


