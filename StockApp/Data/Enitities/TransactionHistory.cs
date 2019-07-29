using Data.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockApp.Data.Enitities
{
    [Table("Transactions")]
    public class TransactionHistory : Audit
    {
        // TransacId, Type, ProductId, Quantity, InvoiceChange, CumQty, UserId, Remarks							
        public int TransactionHistoryId  { get; set; }

        [StringLength(50)]
        public string Type { get; set; }

        public int Quantity { get; set; }
        public int InvoiceChange { get; set; } // Invoice change (RECEIVED or ISSUES items)
        public int CumQty { get; set; } // Quantity after invoice change

        [StringLength(500)]
        public string Remarks { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser User { get; set; } 
    }
}
