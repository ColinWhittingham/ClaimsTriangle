using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsTriangle.Models
{
    public class Product
    {
        public string Name { get; set; }
        public List<OriginYear> OriginYears { get; set; }

        public Product()
        {
            OriginYears = new List<OriginYear>();
        }
    }
}
