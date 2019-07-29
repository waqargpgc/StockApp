using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Data.Enitities
{
    [Table("Products")]
    public class Product : Audit
    {
        // ProductId, Name, SKU, SupplierId, StockLevel, MinStockLevel, CosttPrice, SalePrice, Location, ProductTypeId						

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [StringLength(250)]
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

        public int UnitsInStock { get; set; } // copy inventory initial data from this
        public int MinStockLevel { get; set; } // Minimum Stock Count

        public decimal BuyingPrice { get; set; }
        public decimal SellingPrice { get; set; } 

        [StringLength(250)]
        public string Tags { get; set; } 
        public bool IsDiscontinued { get; set; }
       
        //public int? InventoryLocationId  { get; set; }
        //public InventoryLocation Location { get; set; }

        public int? SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        public bool IsParent { get; set; }
        public int? ParentId { get; set; }
        public Product Parent { get; set; }

        public int? ProductCategoryId  { get; set; }
        public ProductCategory ProductCategory { get; set; }

        public int? ManufacturerId { get; set; } 
        public Manufacturer Manufacturer { get; set; }


        public IList<Inventory> Inventories { get; set; } = new List<Inventory>();
        public IList<OrderDetail>  OrderDetailList { get; set; } = new List<OrderDetail>();
        public IList<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Product> Children { get; set; }
    }
}
 