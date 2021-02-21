using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Task_02
{
    /// <summary>
    /// Class for Collection on list representation.
    /// Contain method for performing task.
    /// </summary>
    /// <remarks>This class can read-write json, search, sort, delete, add, edit object.</remarks>
    public class MyCollection<T> where T : BaseProduct, new()
    {
        private List<T> _data;

        /// <summary>Initializes a new "MyCollection" object.</summary>
        public MyCollection()
        {
            _data = new List<T>();
        }

        /// <summary> Returns a String which represents the object instance.</summary>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(_data, Formatting.Indented);
        }
        
        #region Validate/Check
        /// <summary>Validate object.</summary>
        /// <param name="newJsonAddress">New object</param>
        /// <returns>True if object is valid.</returns>
        /// <exception cref="ValidationException">
        /// Thrown string with all error message when one or more Address parameters is invalid. 
        /// </exception>
        static bool ValidateObject(T newJsonAddress)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(newJsonAddress);
            
            if (!Validator.TryValidateObject(newJsonAddress, context, results, true))
            {
                throw new ValidationException(results.Aggregate("",
                        (current, error) => current + (error.ErrorMessage) +
                                            "\nYour value: " +
                                            typeof(T).GetProperty(error.ErrorMessage?.Split(' ')[0])
                                                ?.GetValue(newJsonAddress, null) + "\n")
                );
            }

            return true;
        }
        
        /// <summary>Check property.</summary>
        /// <param name="property">String representation of T property.</param>
        /// <param name="hideParam">If property == one of this param, return false</param>
        /// <returns>True if property is valid.</returns>
        /// <exception cref="ArgumentException">Invalid parameter</exception>
        private static bool CheckProperty(string property, string[] hideParam = null)
        {
            if (!typeof(T).GetProperties().All(obj => property != obj.Name) & 
                (hideParam ?? Array.Empty<string>()).All(obj=>obj != property)) return true;
            
            throw new ArgumentException($"\"{property}\" is invalid.");
        }
        
        /// <summary>Check id.</summary>
        /// <param name="objId">String representation of object id which should be checked.</param>
        /// <returns>True if id is valid.</returns>
        /// <exception cref="ArgumentException">Invalid id</exception>
        private bool CheckId(Guid objId)
        {
            if (_data.Any(obj => obj.Id == objId)) return true;
            
            throw new ArgumentException($"\"{objId}\" id is invalid.");
        }
        #endregion
        
        #region Json
        /// <summary>Read json file and added new object to collection.</summary>
        /// <param name="fileName">String representation of file name which contained in "resources" folder.</param>
        public void ReadJson(string fileName = "data.json")
        {
            _data = new List<T>();
            
            using (var r = new StreamReader(@"../../../resources/" + fileName))
            {
                foreach (var i in JsonConvert.DeserializeObject<List<T>>(r.ReadToEnd()))
                {
                    try
                    {
                        if (ValidateObject(i)) _data.Add(i);
                    }
                    catch (ValidationException e)
                    {
                        Console.WriteLine("\nValidation error:\n");
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }

        /// <summary>Write new objects from collection to json file.</summary>
        /// <param name="fileName">String representation of file name which contained in "resources" folder.</param>
        public void WriteInFile(string fileName = "data.json")
        {
            using (StreamWriter sw = new StreamWriter(@"../../../resources/" + fileName,
                    false, System.Text.Encoding.Default))      
            {
                sw.Write(this.ToString());
            }
        }
        #endregion

        #region Task method
        /// <summary>Search in collection objects by string representation of specified value.</summary>
        /// <param name="searchValue">String representation of search value.</param>
        /// <returns>List of objects with the found value</returns>
        public List<T> Search(string searchValue)
        {
            return _data.Where(obj => typeof(T).GetProperties()
                    .Select(attr => typeof(T).GetProperty(attr.Name).GetValue(obj, null))
                    .Any(temp => temp.ToString().ToLower().Contains(searchValue.ToLower())))
                .ToList();
        }

        /// <summary>Sort collection by address property.</summary>
        /// <param name="sortBy">String representation of T property. Sorted by this param.</param>
        public void Sort(string sortBy)
        {
            if (!CheckProperty(sortBy)) return;
            
            var properties = typeof(T).GetProperty(sortBy);
                
            _data.Sort((address, address1) => string.Compare(
                properties.GetValue(address, null).ToString().ToLower(),
                properties.GetValue(address1, null).ToString().ToLower(),
                StringComparison.Ordinal)
            );
        }

        /// <summary>Delete address object from collection by id.</summary>
        /// <param name="objId">Id of address object which must be removed.</param>
        /// <exception cref="ArgumentException">Invalid Id.</exception>
        public void Delete(Guid objId)
        {
            if (!_data.Remove(_data.Find(obj => obj.Id == objId)))
            {
                throw new ArgumentException("No address with such ID found");
            }
        }
    
        /// <summary>Add new object to collection.</summary>
        public void AddNewObj()
        {
            var newObj = new T();

            foreach (var attr in typeof(T).GetProperties().Where(obj=>obj.Name != "Id"))
            {
                Console.Write("Enter {0}: ",attr.Name);
                var strValue = Console.ReadLine();
                var properties = typeof(T).GetProperty(attr.Name);
                
                properties.SetValue(newObj, Convert.ChangeType(strValue, properties.PropertyType), null);
            }
            if (ValidateObject(newObj)) _data.Add(newObj); 
        }

        /// <summary>Edit object.</summary>
        /// <param name="objId">Object id which must be edited.</param>
        /// <param name="param">Object property which must be edited.</param>
        /// <param name="value">New object value.</param>
        /// <exception cref="ArgumentException">Invalid object id, parameter or new value</exception>
        public void EditObject(Guid objId, string param, string value)
        {
            var hiddenProperty = new[] {"Id"};
            
            if (CheckProperty(param, hiddenProperty) & CheckId(objId))
            {
                var properties = typeof(T).GetProperty(param);
                var currentObj = _data.Find(obj => obj.Id == objId);
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
            else throw new ArgumentException("Invalid data!"); 
        }
        #endregion
    }
}