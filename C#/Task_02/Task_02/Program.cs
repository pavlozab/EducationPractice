using System;
using System.Linq;
using Newtonsoft.Json;

namespace Task_02
{
    class Program
    {
        static void Main(string[] args)
        {
            string choice, fileName = "data.json";
            var collection = new MyCollection<Address>();
            do
            {
                PrintHelp();
                choice = Console.ReadLine();
                try
                {
                    switch (choice)
                    {
                        case "1": collection.ReadJson(fileName);
                            break;
                        case "2": Search<Address>(collection);
                            break;
                        case "3": SortBy<Address>(collection, fileName);
                            break;
                        case "4": Delete<Address>(collection, fileName);
                            break;
                        case "5":
                            collection.AddNewObj();
                            collection.WriteInFile(fileName);
                            break;
                        case "6": Edit<Address>(collection, fileName);
                            break;
                        case "7": Console.WriteLine(collection.ToString());
                            break;
                        case "8": fileName = FileName();
                            break;
                        case "exit": Console.WriteLine("Goodbye!");
                            break;
                        default: Console.WriteLine("Wrong input!");
                            break;
                    }
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine("\nArgument error:\n");
                    Console.WriteLine(e.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine("\nError:");
                    Console.WriteLine(e.Message);
                }
            } while ( choice != "exit");
        }
            
        #region MenuFunc
        /// <summary>The function that returns help message.</summary>
        static void PrintHelp()
        {
            Console.WriteLine("\n- - - - - - - - - - - - - - - - - -");
            Console.WriteLine("| Help:                           |");
            Console.WriteLine("| 1 - to read from file.          |");
            Console.WriteLine("| 2 - to search.                  |");
            Console.WriteLine("| 3 - to sort by.                 |");
            Console.WriteLine("| 4 - to delete.                  |");
            Console.WriteLine("| 5 - to add new.                 |");
            Console.WriteLine("| 6 - to edit element.            |");
            Console.WriteLine("| 7 - to print collection.        |");
            Console.WriteLine("| 8 - change file name.           |");
            Console.WriteLine("|  exit - to exit.                |");
            Console.WriteLine("- - - - - - - - - - - - - - - - - -\n");
        }
        
        /// <summary>The function that returns string representation of possible address parameter.</summary>
        static void WritePossibleParameter<T>()
        {
            Console.Write("\nPOSSIBLE: ");
            
            foreach (var i in typeof(T).GetProperties())
            {
                Console.Write("{0}, ", i.Name);
            }
            Console.WriteLine();
        }

        static string FileName()
        {
            Console.WriteLine("Enter file name: ");
            var fileName = Console.ReadLine();
            return fileName;
        }

        static void Search<T>(MyCollection<T> collection) where T : BaseProduct, new()
        {
            Console.Write("Enter parameter which elements you want to find:  ");
            var searchValue = Console.ReadLine();
            
            var searchRes =  collection.Search(searchValue);
            
            if (searchRes.Any()) Console.WriteLine(JsonConvert.SerializeObject(searchRes, Formatting.Indented));
            else
            {
                Console.WriteLine("\nWe couldn't find for {0}. \nTry different or less specific keywords.",
                    searchValue);
            }
        }

        static void SortBy<T>(MyCollection<T> collection, string fileName) where T : BaseProduct, new()
        {
            Console.Write("Enter field for which you want to sort:");
            WritePossibleParameter<T>();
            var searchValue = Console.ReadLine();
            collection.Sort(searchValue);
            collection.WriteInFile(fileName);
        }

        static void Delete<T>(MyCollection<T> collection, string fileName) where T : BaseProduct, new()
        {
            Console.WriteLine("Enter id to delete: ");
            var id = Guid.Parse(Console.ReadLine() ?? string.Empty);
            collection.Delete(id);
            collection.WriteInFile(fileName);
        }
        
        static void Edit<T>(MyCollection<T> collection, string fileName) where T : BaseProduct, new()
        {
            Console.WriteLine("Enter id to edit: ");
            var id = Guid.Parse(Console.ReadLine() ?? string.Empty);
        
            Console.Write("Enter param to edit: ");
            WritePossibleParameter<T>();
            var param = Console.ReadLine();
            
            Console.WriteLine("Enter value to change: ");
            var value = Console.ReadLine();
            
            collection.EditObject(id, param, value);
            collection.WriteInFile(fileName);
        }
        #endregion
    }
}