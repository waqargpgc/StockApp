using StockApp.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockApp.Data.Enitities
{
    [Table("Orders")]
    public class Order : Audit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        [StringLength(50)]
        public string OrderReference { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }

        [StringLength(2000)]
        public string Comments { get; set; }
        public int? CustomerId { get; set; }
        public Customer Customer { get; set; }

        [EnumDataType(typeof(Enumerations.OrderStatus))]
        public Enumerations.OrderStatus Status { get; set; }
        public ICollection<OrderDetail> OrderDetailList { get; set; }

        // Destructure data from customer
        // needed for instant transaction - a Customer may not always be registered/added.
        [StringLength(250)]
        public string CustName { get; set; } 

        [MaxLength(500)]
        public string CustAddress { get; set; } 

        [MaxLength(30)]
        public string CustMobile { get; set; }

        [MaxLength(100)]
        public string CustEmail { get; set; }

    }
}
