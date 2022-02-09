using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string paramFile = args[0];
            
            // This text is added only once to the file.
            if (File.Exists(paramFile))
            {
                string jsonParams = File.ReadAllText(paramFile);
               
                var logger = new ConsoleLogger();
                var scaler = new ScScaler(jsonParams, logger);

                try
                {
                    scaler.ScaleImages();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);                    
                }

                
            }
                                 
        }
    }
}
