using ClaimsTriangle.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsTriangle
{
    class Program
    {
        static void Main(string[] args)
        {
            var importPath = $@"{Helper.GetDefaultDirectory()}\Content\input.csv";
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("NOTE: File must be in CSV (comma delimited) format and a header row is expected.");
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine($"Attempting to import file with default name and location {importPath}.");
            Console.WriteLine("Press 'Enter' to proceed. Any other input will be used as the file path.");
            var inputString = Console.ReadLine();
            while (!string.IsNullOrEmpty(inputString))
            {
                importPath = inputString;
                Console.WriteLine($"Updated import path to {importPath}. Press 'Enter' to proceed or re-enter path.");
                inputString = Console.ReadLine();
            }

            var validationService = new ValidationService();
            var validationResults = new List<string>();
            var products = validationService.Invoke(importPath, validationResults);

            if (validationResults.Any())
            {
                Console.WriteLine("File failed validation. Please correct the following errors:");
                foreach(string error in validationResults)
                {
                    Console.WriteLine(error);
                }
                Console.WriteLine();
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
                return;
            }
            else
            {
                var accumulationService = new AccumulationService();
                foreach(var product in products)
                {
                    accumulationService.Invoke(product);
                }
                var exportService = new ExportService();
                exportService.Invoke(products);
                Console.WriteLine("Created export file.");
                Console.WriteLine();
                Console.WriteLine("Press any key to exit.");
            }
            Console.ReadLine();
        }
    }
}
