using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Helpers
{
    public static class Enumerations
    {
       public enum OrderStatus
        {
            Draft = 0,
            Released = 1,
            Completed = 2
        };

        public enum IsNewOrder 
        {
            NO  = 0,
            YES = 1
        };
    }
}
