using Doerly.Module.Catalog.DataAccess.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ServiceFilterValueEntity = Doerly.Module.Catalog.DataAccess.Models.ServiceFilterValue;

namespace Doerly.Module.Catalog.DataAccess.EntityConfiguration
{
    public class ServiceFilterValueConfiguration : IEntityTypeConfiguration<ServiceFilterValueEntity>
    {
        public void Configure(EntityTypeBuilder<ServiceFilterValueEntity> builder)
        {
            builder.ToTable(DbConstants.Tables.ServiceFilterValue, DbConstants.CatalogSchema);

            builder.HasKey(sfv => sfv.Id);

            builder.Property(sfv => sfv.Value)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasOne(sfv => sfv.Service)
                .WithMany(s => s.FilterValues)
                .HasForeignKey(sfv => sfv.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(sfv => sfv.Filter)
                .WithMany(f => f.FilterValues)
                .HasForeignKey(sfv => sfv.FilterId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(sfv => sfv.ServiceId);
            builder.HasIndex(sfv => sfv.FilterId);
        }
    }
}
