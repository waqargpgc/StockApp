using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Data.Enitities
{
    [Table("InventoryLocations")]
    public class InventoryLocation : Audit
    {
        [Key]
        public int InventoryLocationId { get; set; }

        [StringLength(50)]
        public string LocationName { get; set; }

        [StringLength(1000)]
        public string Description { get; set; } 
    }
}
