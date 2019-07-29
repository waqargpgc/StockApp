using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Data.Enitities
{
    [Table("ProductCategory")]
    public class ProductCategory : Audit
    {
        // ProductTypeId, TypeName, TypeDescription						

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(2500)]
        public string Description { get; set; }

        [StringLength(150)]
        public string Icon { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
