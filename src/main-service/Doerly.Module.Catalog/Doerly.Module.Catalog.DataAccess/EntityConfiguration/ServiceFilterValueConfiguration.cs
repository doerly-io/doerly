using Doerly.Module.Catalog.DataAccess.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceFilterValueEntity = Doerly.Module.Catalog.DataAccess.Models.ServiceFilterValue;

namespace Doerly.Module.Catalog.DataAccess.EntityConfiguration
{
    public class ServiceFilterValueConfiguration : IEntityTypeConfiguration<ServiceFilterValueEntity>
    {
        public void Configure(EntityTypeBuilder<ServiceFilterValueEntity> builder)
        {
            builder.ToTable(DbConstants.Tables.ServiceFilterValue, DbConstants.CatalogSchema);

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Value).IsRequired().HasMaxLength(100);

            builder.HasOne(x => x.Service)
                   .WithMany(x => x.FilterValues)
                   .HasForeignKey(x => x.ServiceId);

            builder.HasOne(x => x.Filter)
                   .WithMany()
                   .HasForeignKey(x => x.FilterId);
        }
    }
}
