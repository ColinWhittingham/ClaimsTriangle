using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsTriangle.Models
{
    public class OriginYear
    {
        public Int16 Year { get; set; }
        public List<DevelopmentYear> DevelopmentYears { get; set; }

        public OriginYear()
        {
            DevelopmentYears = new List<DevelopmentYear>();
        }
    }
}
