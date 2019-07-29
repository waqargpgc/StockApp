using StockApp.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models.Identity
{
    public class UserResourceParameters : ResourceParameters
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
    }
}
