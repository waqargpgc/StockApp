using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Data.Enitities
{
    [Table("Customers")]
    public class Customer : Audit
    { 				
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }

        [MaxLength(150)]
        public string  Name { get; set; }

        [MaxLength(500)]
        public string Address { get; set; }

        [MaxLength(30)]
        public string Telephone { get; set; }

        [MaxLength(30)]
        public string Mobile { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }


        public IList<Order> Orders { get; set; } = new List<Order>(); 
    }
}
