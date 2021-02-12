using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

public class MyCollection
{
    private List<Address> Data;

    public MyCollection()
    {
        this.Data = new List<Address>();
    }

    // Return the point's value as a string.
    public override string ToString()
    {
        return JsonConvert.SerializeObject(Data, Formatting.Indented);
    }

    static bool ValidateObject(Address newJsonAddress)
    {
        string result = "";
        var results = new List<ValidationResult>();
        var context = new ValidationContext(newJsonAddress);

        if (!Validator.TryValidateObject(newJsonAddress, context, results, true))
        {
            foreach (var error in results)
            {
                result += error.ErrorMessage + "\n";
            }

            throw new ArgumentException(result);
            return false;
        }
        return true; 
    }
    
    public void ReadJson(string filePath = "resources/data.json")
    {
        string neededPath = System.IO.Directory.GetCurrentDirectory().Replace("bin/Debug/net5.0", 
            filePath);  // :(
        using (StreamReader r = new StreamReader(neededPath))
        {
            foreach (Address i in JsonConvert.DeserializeObject<List<Address>>(r.ReadToEnd()))
            {
                if (ValidateObject(i))
                {
                    Data.Add(i);
                }
            }
        }
    }
    
    public void WriteInFile(string filePath = "resources/data.json")
    {
        string writePath = System.IO.Directory.GetCurrentDirectory().Replace("bin/Debg/net5.0", 
            filePath);
        using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
        {
            sw.WriteAsync(this.ToString());
        }
    }
    
    public List<Address> Search(string searchValue)
    {
        List<Address> searchResult = new List<Address>();

        foreach (var obj in this.Data)
        {
            foreach (var attr in typeof(Address).GetProperties())
            {
                var temp = typeof(Address).GetProperty(attr.Name).GetValue(obj, null);
                if (temp.ToString().ToLower().Contains(searchValue)) { searchResult.Add(obj); continue; }
            }
        }
        
        return searchResult;
    }

    private bool ValidParametr(string param)
    {
        if (!typeof(Address).GetProperties().Any(obj => param == obj.Name))
        {
            throw new Exception($"{param} is invalid.");
            return false;
        }

        return true;
    }
    
    public void Sort(string sortBy)
    {
        if (ValidParametr(sortBy))
        {
            Data.Sort(delegate(Address address, Address address1)
            {
                return String.Compare(
                    typeof(Address).GetProperty(sortBy).GetValue(address, null).ToString().ToLower(),
                    typeof(Address).GetProperty(sortBy).GetValue(address1, null).ToString().ToLower(),
                    StringComparison.Ordinal);
            });
        }
    }

    public void Delete(int id)
    {
        if (!Data.Remove(this.Data.Find(obj => obj.Id == id)))
        {
            throw new Exception("No adddress with such ID found");
        }
    }
    
    public void AddNewObj()
    {
        Address newObj = new Address(this.Data.Max(obj => obj.Id)+1);
        
        foreach (var attr in typeof(Address).GetProperties().Skip(1))
        {
            Console.Write("Enter {0}: ", attr.Name);
            string strValue = Console.ReadLine();
            typeof(Address).GetProperty(attr.Name).SetValue(newObj, strValue, null);
        }
        if (ValidateObject(newObj)) { Data.Add(newObj); }
    }

    public void EditeObj(int objId, string param, object value)
    {

        if (!ValidParametr(param) & !Data.Any(obj => obj.Id == objId))
        {
            throw new ArgumentException("Invalid data!");
        }
        else
        {
            string result = "";
        
            var results = new List<ValidationResult>();
            var context = new ValidationContext(typeof(Address).GetProperty(param));

            if (!Validator.TryValidateObject(value, context, results, true))
            {
                foreach (var error in results) { result += error.ErrorMessage + "\n"; }
                throw new ArgumentException("result");
            }
            else
            {
                typeof(Address).GetProperty(param).SetValue(
                    Data.Find(obj => obj.Id == objId), 
                    value, null);
            }
        }
    }

    
    
}


