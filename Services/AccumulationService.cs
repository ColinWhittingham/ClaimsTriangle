using ClaimsTriangle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsTriangle.Services
{
    public class AccumulationService
    {
        public void Invoke(Product product)
        {
            foreach (var originYear in product.OriginYears)
            {
                var accumulatedTotal = 0m;
                foreach ( var developmentYear in originYear.DevelopmentYears)
                {
                    accumulatedTotal += developmentYear.IncrementalValue;
                    developmentYear.AccumulatedValue = accumulatedTotal;
                }
            }
        }
    }
}
