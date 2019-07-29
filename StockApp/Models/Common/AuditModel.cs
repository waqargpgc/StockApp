using System;
using System.ComponentModel.DataAnnotations;

namespace StockApp.Models
{
    public class AuditModel
    {
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        [MaxLength(256)]
        public string CreatedBy { get; set; }

        [MaxLength(256)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public DateTime? CreatedDate { get; set; }

        [StringLength(450)]
        public string SharedKey { get; set; }
    }
}
