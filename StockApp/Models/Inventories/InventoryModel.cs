using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace StockApp.Models
{
    public class InventoryModel : AuditModel
    {
        public int InventoryId { get; set; }

        [StringLength(250)]
        public string Tags { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }
        public int StockLevel { get; set; } 

        public int? InventoryLocationId { get; set; }
        public int ProductId { get; set; }
         
    }
}
