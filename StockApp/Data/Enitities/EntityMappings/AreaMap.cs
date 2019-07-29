using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Data.Enitities.EntityMappings
{
    public class AreaMap : IEntityTypeConfiguration<Area>
    {
        public List<Area> DefaultData = new List<Area>
        {
            new Area{AreaId = 1, Name = "Default Area", Description = "Default Area", IsActive = true, IsDeleted = false },
        };

        public void Configure(EntityTypeBuilder<Area> builder)
        {
            builder.HasData(DefaultData);
        }
    }
}
