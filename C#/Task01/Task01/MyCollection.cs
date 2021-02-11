using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using Newtonsoft.Json.Linq;   
// FIXME delete using witch not used 

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
        else { this.Data.Add(newJsonAddress); }
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

    // функція повертає true, коли параметр належить до проепрті об'єкту 
    private bool ValidParametr(string param)
    {
        return typeof(Address).GetProperties().Any(obj => param == obj.Name);
    }
    public void Sort(string sortBy)
    {
        // провірка правильності вказаного поля пошуку
        if (!this.ValidParametr(sortBy))
        {
            Console.WriteLine("{0} is invalid. ", sortBy);
        }
        else
        {
            this.Data.Sort(delegate(Address address, Address address1)
            {
                return String.Compare(
                    typeof(Address).GetProperty(sortBy).GetValue(address, null).ToString().ToLower(),
                    typeof(Address).GetProperty(sortBy)?.GetValue(address1, null).ToString().ToLower(),
                    StringComparison.Ordinal);
            });

        }
    }

    public void Delete(int id)
    {
        bool idCheck = false;
        
        foreach (var i in this.Data)
        {
            if (i.Id == id)
            {
                this.Data.Remove(i);
                idCheck = true;
                break;
            }
        }

        if (!idCheck) { Console.WriteLine("No adddress with such ID found"); } // FIXME raise exception ?}
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
        this.ValidateAddObject(newObj);
    }

    public void EditeObj()
    {
        Console.WriteLine("Enter id to edit: ");
        int id = Int32.Parse(Console.ReadLine());
        
        Console.WriteLine("Enter param to edit: ");
        string param = Console.ReadLine();

        if (!this.ValidParametr(param)) { Console.WriteLine("{0} is invalid.", param); } // FIXME exception}
        else
        {
            Console.WriteLine("Enter value to change: ");
            string value = Console.ReadLine();
            
            // FIXME видалити по id
            //this.Data.IndexOf(obj=>obj.Id ==id);
        }
    }
    
}


