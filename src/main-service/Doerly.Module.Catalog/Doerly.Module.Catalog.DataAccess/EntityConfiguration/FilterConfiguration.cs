using Doerly.Module.Catalog.DataAccess.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using FilterEntity = Doerly.Module.Catalog.DataAccess.Models.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doerly.Module.Catalog.DataAccess.EntityConfiguration
{
    public class FilterConfiguration : IEntityTypeConfiguration<FilterEntity>
    {
        public void Configure(EntityTypeBuilder<FilterEntity> builder)
        {
            builder.ToTable(DbConstants.Tables.Filter, DbConstants.CatalogSchema);

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Type).IsRequired().HasMaxLength(50);
            builder.Property(x => x.IsCustomInput).IsRequired();

            builder.HasOne(x => x.Service)
                   .WithMany(x => x.Filters)
                   .HasForeignKey(x => x.ServiceId);
        }
    }
}
