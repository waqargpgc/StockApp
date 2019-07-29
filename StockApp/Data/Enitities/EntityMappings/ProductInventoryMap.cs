using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Data.Enitities.EntityMappings
{
    //public class ProductInventoryMap : IEntityTypeConfiguration<ProductInventory>
    //{
    //    //public List<Inventory> DefaultData = new List<Inventory>
    //    //        {
    //    //            new Inventory{InventoryId = 1, Description = "Self" },
    //    //            new Inventory{InventoryId = 2, Description = "Self" }
    //    //        };


    //    public void Configure(EntityTypeBuilder<ProductInventory> builder)
    //    {
    //        //builder.HasData(DefaultData); 
    //        builder.HasKey(pi => new { pi.ProductId, pi.InventoryId });

    //        builder.HasOne<Inventory>(sc => sc.Inventory)
    //            .WithMany(s => s.ProductInventories)
    //            .HasForeignKey(sc => sc.InventoryId);

    //        builder.HasOne<Product>(sc => sc.Product)
    //            .WithMany(s => s.ProductInventories)
    //            .HasForeignKey(sc => sc.ProductId);
    //    }
    //}
}

