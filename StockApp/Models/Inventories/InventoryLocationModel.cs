using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Models
{
    public class InventoryLocationModel : AuditModel
    {
        public int InventoryLocationId { get; set; }

        [StringLength(50)]
        public string LocationName { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }
    }
}
