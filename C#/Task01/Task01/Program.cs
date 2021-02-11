using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace Task01
{
    class Program
    {
        static void Main(string[] args)
        {
            MyCollection myCollection = new MyCollection();
            myCollection.ReadJson("data.json");
            
            Console.Write(myCollection.ToString());


            myCollection.Search();





            /*
            foreach (var i in typeof(Address).GetProperties())
            {
                Console.WriteLine(i);
            }
            try
            
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
                Console.WriteLine("Method: {0}", ex.TargetSite);
                Console.WriteLine("Trace stack: {0}", ex.StackTrace);
            }*/

            //Console.Read();

            /*int[] myIntArray = new int[5];

            myIntArray.Average();

            string nums = Console.ReadLine();
            //int[] data =  new int[nums.Split().Length] { foreach(string i in nums.Split()) }
            
            //int []l2 = nums.Where(num=>num.Tont32())

            int size = 7;
            int[][] data = MatrixCreate();*/



        }
    }
    
}