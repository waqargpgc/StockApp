using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Data.Enitities.EntityMappings
{
    public class InventoryLocationMap : IEntityTypeConfiguration<InventoryLocation>
    {
        public List<InventoryLocation> DefaultData = new List<InventoryLocation>
        {
            new InventoryLocation{InventoryLocationId = 1, LocationName = "Default Location", Description = "Default Location" },
        };

        public void Configure(EntityTypeBuilder<InventoryLocation> builder)
        {
            builder.HasData(DefaultData);
        }
    }
}
