using ClaimsTriangle.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsTriangle.Services
{
    public class ValidationService
    {
        private const bool useHeaders = true; //Could make app key setting
        private const char delimiter = ','; //Could make app key setting

        public IEnumerable<Product> Invoke(string filePath, List<string> validationResults)
        {
            //new-ing this because no DI implemented
            var importService = new ImportService();
            List<Product> productResults = null;

            try
            {
                //is the file csv
                if (!Path.GetExtension(filePath).Equals(".csv", StringComparison.OrdinalIgnoreCase))
                {
                    validationResults.Add("File is not a CSV.");
                }
            }
            catch
            {
                //does the path throw an error
                validationResults.Add("Unable to read file path.");
            }

            if (!validationResults.Any())
            {
                //does file exist
                if (!File.Exists(filePath))
                {
                    validationResults.Add("No file found in location provided");
                }
                else
                {
                    var fileLines = new List<string>();

                    try
                    {
                        using (var reader = new StreamReader(filePath))
                        {
                            if (useHeaders) reader.ReadLine();

                            while (!reader.EndOfStream)
                            {
                                fileLines.Add(reader.ReadLine());
                            }
                        }
                    }
                    catch
                    {
                        //catch exceptions during read
                        validationResults.Add("Error when reading file");
                    }

                    if (fileLines.Count == 0)
                    {
                        //has at least one row after heading
                        validationResults.Add("CSV file contains no data");
                    }
                    else
                    {
                        short maxOriginYear = 0;
                        short maxDevelopmentYear = 0;

                        var tableFromCSV = new DataTable();
                        tableFromCSV.Columns.Add(new DataColumn("Product", typeof(string)));
                        tableFromCSV.Columns.Add(new DataColumn("OriginYear", typeof(Int16)));
                        tableFromCSV.Columns.Add(new DataColumn("DevelopmentYear", typeof(Int16)));
                        tableFromCSV.Columns.Add(new DataColumn("IncrementalValue", typeof(decimal)));

                        try
                        {
                            foreach (var fileLine in fileLines)
                            {
                                var values = fileLine.Split(delimiter);
                                //has exactly 4 columns
                                if (values.Length != 4)
                                {
                                    validationResults.Add("One or more lines do not contain 4 columns");
                                    break;
                                }
                                else
                                {
                                    if (!string.IsNullOrWhiteSpace(values[0]))
                                    {
                                        if (IsValidOriginYear(values[1], out short originYear))
                                        {
                                            maxOriginYear = originYear > maxOriginYear ? originYear : maxOriginYear;
                                            if (IsValidDevelopmentYear(values[2], originYear, out short developmentYear))
                                            {
                                                maxDevelopmentYear = developmentYear > maxDevelopmentYear ? developmentYear : maxDevelopmentYear;
                                                if (decimal.TryParse(values[3], out decimal incrementalValue))
                                                {
                                                    tableFromCSV.Rows.Add(values[0], originYear, developmentYear, incrementalValue);
                                                }
                                                else
                                                {
                                                    validationResults.Add("One or more incremental values are invalid");
                                                }
                                            }
                                            else
                                            {
                                                validationResults.Add("One or more development years are invalid");
                                            }
                                        }
                                        else
                                        {
                                            validationResults.Add("One or more origin years are invalid");
                                        }
                                    }
                                    else
                                    {
                                        validationResults.Add("One or more product names are blank");
                                    }
                                }
                            }
                        }
                        catch
                        {
                            //catch exceptions during delimiting
                            validationResults.Add("Error when reading delimited line values");
                        }

                        if (maxDevelopmentYear > maxOriginYear)
                        {
                            validationResults.Add("File contains development data for years without origin");
                        }

                        if (!validationResults.Any())
                        {
                            productResults = importService.Invoke(tableFromCSV).ToList();
                        }
                    }
                }
            }

            return productResults;
        }

        private bool IsValidOriginYear(string input, out short result)
        {
            //assumed year is between 1700 and 2100 for purposes of this app
            return short.TryParse(input, out result) && result > 1700 && result < 2100;
        }

        private bool IsValidDevelopmentYear(string input, short originYear, out Int16 result)
        {
            return short.TryParse(input, out result) && result >= originYear && result < 2100;
        }
    }
}
