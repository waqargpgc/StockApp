using StockApp.Data.Enitities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Models
{
    public class ProductDetailModel : AuditModel
    {
        public int ProductId { get; set; }

        [StringLength(250)]
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [StringLength(3000)]
        public string Description { get; set; }

        [StringLength(100)]
        public string Icon { get; set; }

        [StringLength(30)]
        public string SKU { get; set; } // Stock Keeping Unit

        [StringLength(30)]
        public string UPC { get; set; } // Universal Product Code 

        [StringLength(500)]
        public string SNO { get; set; } // Serial Numbers // -- Serial Numbers separated with comma

        public int CurrentStockLevel { get; set; }
        public int StockLocationCount { get; set; }
        public int MinStockLevel { get; set; } // Minimum Stock Count

        public decimal BuyingPrice { get; set; }
        public decimal SellingPrice { get; set; }

        [StringLength(250)]
        public string Tags { get; set; }
        public string CategoryName { get; set; }
     
        public bool IsDiscontinued { get; set; }  

        public int? SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        public int? ParentId { get; set; }
        public Product Parent { get; set; }


        public int? ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; }

        //public ICollection<ProductInventory> ProductInventories { get; set; } = new List<ProductInventory>();
        ////public ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

        //public IList<OrderDetail> OrderDetailList { get; set; } = new List<OrderDetail>();
        //public IList<Order> Orders { get; set; } = new List<Order>();
        //public ICollection<Product> Children { get; set; }
    }
}
