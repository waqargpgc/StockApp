using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Data.Enitities
{
    [Table("Suppliers")]
    public class Supplier : Audit
    {
        // SupplierId, FirstName, LastName, Address, Mobile, Phone

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SupplierId { get; set; }

        [StringLength(250)]
        public string FirstName { get; set; }

        [StringLength(250)]
        public string LastName { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        [StringLength(30)]
        public string Mobile { get; set; }

        [MaxLength(30)]
        public string Phone { get; set; } 
     
        public IList<Product> Products { get; set; } = new List<Product>();
    }
}
