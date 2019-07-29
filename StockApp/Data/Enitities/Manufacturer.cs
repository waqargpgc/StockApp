using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Data.Enitities
{
    [Table("Manufacturers")]
    public class Manufacturer : Audit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ManufacturerId { get; set; } 

        [StringLength(150)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; } 
    }
}
