using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Data.Enitities.EntityMappings
{
    public class InventoryMap : IEntityTypeConfiguration<Inventory>
    {
        public void Configure(EntityTypeBuilder<Inventory> builder)
        {
            // __:: TOOD - configure composite key on productId & location Id
            // __:: TOOD - Prevent Inventory duplication 

            builder.HasIndex(p => new { p.ProductId, p.InventoryLocationId}).IsUnique();
        }
    }
}
