using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Newtonsoft.Json;
using System.IO;
using System.Reflection.Metadata;

public class MyCollection
{
    private List<Address> Data;

    public MyCollection()
    {
        this.Data = new List<Address>();
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(Data, Formatting.Indented);
    }

    private void ValidateAddObject(Address newJsonAddress)
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
        }
        else
            this.Data.Add(newJsonAddress);
    }
    
    public void ReadJson(string filePath)
    {
        // FIXME розібратись з path
        using (StreamReader r = new StreamReader("/Users/pasha/Documents/GitHub/PythonGit/PythonProgramming/C#/Task01/Task01/resources/" + filePath))
        {
            string json = r.ReadToEnd();
            var newJsonAddress = JsonConvert.DeserializeObject<List<Address>>(json); // Nedded to validate new addresses.

            foreach (Address i in newJsonAddress)
            {
                this.ValidateAddObject(i);
            }
        }
    }

    public Address Search()
    {
        foreach (var i in typeof(Address).GetProperties())
        {
            Console.WriteLine(i.SetMethod);
        }
        
        return this.Data[0];
    }
    
}


