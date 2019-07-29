using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Models
{
    public class ProductListModel : AuditModel
    {
        public int ProductId { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string SKU { get; set; } // Stock Keeping Unit  
        public string Location { get; set; }
        public string Tags { get; set; }
        // eg: 300 in 4 locations 
        public int CurrentStockLevel { get; set; }
        public int StockLocationCount { get; set; }
        public int MinStockLevel { get; set; }

        // Info for filter & search 
        public bool IsDiscontinued { get; set; }
        public string SearchTerm { get; set; }
        public string CategoryName { get; set; }

        public decimal BuyingPrice { get; set; }
        public decimal SellingPrice { get; set; } 
        public string ManufacturerName { get; set; }

        // List & for filter __:: TODO Add method for each list in repo
        public IList<ProductListModel> TrashedProductList { get; set; } = new List<ProductListModel>();
        public IList<ProductListModel> ActiveProductList { get; set; } = new List<ProductListModel>();
        public IList<ProductListModel> InAtiveProductList { get; set; } = new List<ProductListModel>();
        public IList<ProductListModel> ProductList { get; set; } = new List<ProductListModel>();
    }


}
