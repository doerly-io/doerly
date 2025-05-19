using Doerly.Module.Catalog.DataAccess.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceEntity = Doerly.Module.Catalog.DataAccess.Models.Service;

namespace Doerly.Module.Catalog.DataAccess.EntityConfiguration
{
    public class ServiceConfiguration : IEntityTypeConfiguration<ServiceEntity>
    {
        public void Configure(EntityTypeBuilder<ServiceEntity> builder)
        {
            builder.ToTable(DbConstants.Tables.Service, DbConstants.CatalogSchema);

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Description).HasMaxLength(1000);
            builder.Property(x => x.Price).HasPrecision(15, 2);
            builder.Property(x => x.IsDeleted).IsRequired();
            builder.Property(x => x.UserId);

            builder.HasOne(x => x.Category)
                   .WithMany(x => x.Services)
                   .HasForeignKey(x => x.CategoryId);

            builder.HasMany(x => x.Filters)
                   .WithOne(x => x.Service)
                   .HasForeignKey(x => x.ServiceId);

            builder.HasMany(x => x.FilterValues)
                   .WithOne(x => x.Service)
                   .HasForeignKey(x => x.ServiceId);
        }
    }
}
