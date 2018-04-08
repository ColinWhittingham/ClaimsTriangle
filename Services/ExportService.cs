using ClaimsTriangle.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsTriangle.Services
{
    public class ExportService
    {
        private readonly string exportPath = $@"{Helper.GetDefaultDirectory()}\Content\output.csv";

        public void Invoke(IEnumerable<Product> products)
        {
            using (var streamWriter = File.CreateText(exportPath))
            {
                var firstYear = products.Min(p => p.OriginYears.Min(o => o.Year));
                var lastYear = products.Max(p => p.OriginYears.Max(o => o.Year));
                var yearCount = (lastYear - firstYear) + 1;

                streamWriter.WriteLine($"{firstYear}, {yearCount}");
                
                foreach (var product in products)
                {
                    var accValues = product.OriginYears.SelectMany(o => o.DevelopmentYears).Select(d => d.AccumulatedValue.ToString()).ToArray();
                    streamWriter.WriteLine($"{product.Name} ,{string.Join(", ", accValues)}");
                }
            }
        }
    }
}
