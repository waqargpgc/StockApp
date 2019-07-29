using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Models
{
    public class SaleOrderDetailModel : AuditModel 
    { 
        public int Id { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }  
         
        public int ProductId { get; set; }
        public ProductModel Product { get; set; }

     
        public int OrderId { get; set; }
        public SalesOrderListModel SaleOrder { get; set; }
    }
}
