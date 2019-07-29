using StockApp.Data.Enitities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StockApp.Models 
{
    public class CustomerModel : AuditModel
    { 
        public int CustomerId { get; set; }

        [MaxLength(250)]
        public string Name { get; set; } 

        [MaxLength(500)]
        public string Address { get; set; }

        [MaxLength(30)]
        public string Telephone { get; set; }

        [MaxLength(30)]
        public string Mobile { get; set; } 

        public IList<Order> Orders { get; set; } = new List<Order>();  
    }
}
