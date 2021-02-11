using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;


// FIXME needed better structure, task6-7-8, needed try-catch 

namespace Task01
{
    class Program
    {
        static void Main(string[] args)
        {
            int choice = 9;

            do
            {
                PrintHelp();
                
                
            } while (choice != 9);
            
            MyCollection collection = new MyCollection();
            collection.ReadJson("data.json");
            
            //Console.WriteLine(collection.ToString());
            
            //Search(collection);

            //SortBy(collection);
            Console.WriteLine(collection.ToString());
            
            AddNew(collection);


        }
        
        static void PrintHelp()
        {
            Console.WriteLine("\n* * * * * * * * * * * * * * * *");
            Console.WriteLine("* Help:                       *");
            Console.WriteLine("* 1 - read from file          *");
            Console.WriteLine("* 2 - search                  *");
            Console.WriteLine("* 3 - sort by                 *");
            Console.WriteLine("*  *");
            Console.WriteLine("* * * * * * * * * * * * * * * *\n");
        }
        
        static void Search(MyCollection collection)
        {
            Console.Write("Enter parameter which elements you want to find: \n");
            string searchValue = Console.ReadLine();
            
            List<Address> searchRes =  collection.Search(searchValue);
            if (searchRes.Any())
            {
                foreach (var obj in searchRes)
                {
                    Console.WriteLine(obj.ToString());
                }
            }
            else
            {
                Console.WriteLine("\nWe couldn't find for {0}. \nTry different or less specific keywords.",
                    searchValue);
            }
        }

        static void SortBy(MyCollection collection)
        {
            Console.Write("Enter field for which you want to sort: \nPOSSIBLE: ");
            foreach (var i in typeof(Address).GetProperties())
            {
                Console.Write("{0}, ", i.Name);
            }
            Console.WriteLine();
            string searchValue = Console.ReadLine();
            
            collection.Sort(searchValue);
            Console.WriteLine(collection.ToString());
        }

        static void Delete(MyCollection collection)
        {
            Console.WriteLine("Enter id to delete: ");
            int id = Int32.Parse(Console.ReadLine());
            collection.Delete(id);
            Console.WriteLine(collection.ToString());
        }

        static void AddNew(MyCollection collection)
        {
            collection.AddNewObj();
            Console.WriteLine(collection.ToString());
        }
    }
    
    
    
    
}