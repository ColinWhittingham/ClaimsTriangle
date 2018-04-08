using ClaimsTriangle.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsTriangle.Services
{
    public class ImportService
    {
        public IEnumerable<Product> Invoke(DataTable dataTable)
        {
            var enumerableTable = dataTable.AsEnumerable();
            var productNames = enumerableTable.Select(o => o["Product"].ToString()).Distinct();
            var firstYear = enumerableTable.Min(o => (short)o["OriginYear"]);
            var lastYear = enumerableTable.Max(o => (short)o["OriginYear"]);
            var products = new List<Product>();

            foreach (var productName in productNames)
            {
                var product = new Product { Name = productName };
                var productRows = enumerableTable.Where(o => o["Product"].ToString().Equals(productName, StringComparison.OrdinalIgnoreCase));
                var productFirstYear = productRows.Min(o => (short)o["OriginYear"]);
                //var productLastYear = productRows.Max(o => (short)o["OriginYear"]);
                for (short originYearNum = firstYear; originYearNum <= lastYear; originYearNum++)
                {
                    var originYear = new OriginYear { Year = originYearNum };
                    var counter = (short)0;
                    for (short developmentYearNum = firstYear; developmentYearNum <= lastYear; developmentYearNum++)
                    {
                        var matchingRow = productRows.FirstOrDefault(o => (short)o["OriginYear"] == originYearNum && (short)o["DevelopmentYear"] == developmentYearNum);
                        var incrementalValue = matchingRow == null ? 0m : (decimal)matchingRow["IncrementalValue"];
                        if (developmentYearNum >= originYearNum)
                        {
                            if (developmentYearNum >= productFirstYear && originYearNum >= productFirstYear) counter++;
                            var developmentYear = new DevelopmentYear { DevelopmentYearNumber = counter, IncrementalValue = incrementalValue };
                            originYear.DevelopmentYears.Add(developmentYear);
                        }

                    }
                    product.OriginYears.Add(originYear);
                }
                products.Add(product);
            }

            return products;
        }
    }
}
