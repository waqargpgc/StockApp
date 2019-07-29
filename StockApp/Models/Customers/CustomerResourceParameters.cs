using StockApp.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Models.Customers
{
    public class CustomerResourceParameters : ResourceParameters
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
    }
}
