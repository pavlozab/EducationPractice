using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

// FIXME delete using witch not used 

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

    private bool ValidateObject(Address newJsonAddress)
    {
        var results = new List<ValidationResult>();
        var context = new ValidationContext(newJsonAddress);

        if (!Validator.TryValidateObject(newJsonAddress, context, results, true))
        {
            foreach (var error in results)
            {
                Console.WriteLine(error.ErrorMessage);
                Console.WriteLine(newJsonAddress);
            }
            return false;
        }
        return true; 
    }
    
    public void ReadJson(string filePath = "data.json")
    {
        try
        {
            using (StreamReader r = new StreamReader("/Users/pasha/Documents/GitHub/PythonGit/PythonProgramming/C#/Task01/Task01/resources/" + 
                                                     filePath))
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
        catch (Exception e) { throw; }
        // FIXME розібратись з path
    }
    
    public void WriteInFile(string filePath = "data.json")
    {
        try
        {
            string writePath = "/Users/pasha/Documents/GitHub/PythonGit/PythonProgramming/C#/Task01/Task01/resources/" +
                               filePath;
            using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
            {
                sw.WriteAsync(this.ToString());
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    // метод пошуку повертає список об'єктів, які містять шукане значення
    public List<Address> Search(string searchValue)
    {
        List<Address> searchResult = new List<Address>();

        foreach (var obj in this.Data)
        {
            foreach (var attr in typeof(Address).GetProperties())
            {
                // получаємо значення за назвою поля
                var temp = typeof(Address).GetProperty(attr.Name).GetValue(obj, null);
                
                if (temp.ToString().ToLower().Contains(searchValue)) { searchResult.Add(obj); continue; }
            }
        }
        
        return searchResult;
    }

    // функція повертає true, коли параметр належить до проперті об'єкту 
    private void ValidParametr(string param)
    {
        try
        {
            if (!typeof(Address).GetProperties().Any(obj => param == obj.Name))
            {
                throw new Exception($"{param} is invalid.");
            }
        }
        catch (Exception e) { throw; }
    }
    
    public void Sort(string sortBy)
    {
        ValidParametr(sortBy);
        Data.Sort(delegate(Address address, Address address1)
        {
            return String.Compare(
                typeof(Address).GetProperty(sortBy).GetValue(address, null).ToString().ToLower(),
                typeof(Address).GetProperty(sortBy).GetValue(address1, null).ToString().ToLower(),
                StringComparison.Ordinal);
        });
    }

    public void Delete(int id)
    {
        try
        {
            if (!Data.Remove(this.Data.Find(obj => obj.Id == id)))
            {
                throw new Exception("No adddress with such ID found");
            }
        }
        catch (Exception e) { throw; }
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
        ValidParametr(param);
        typeof(Address).GetProperty(param).SetValue(
            this.Data.Find(obj => obj.Id == objId), 
            value, null);
        
        // FIXME validation for edited object
    }

    
    
}


