using Doerly.Module.Catalog.DataAccess.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using FilterEntity = Doerly.Module.Catalog.DataAccess.Models.Filter;

namespace Doerly.Module.Catalog.DataAccess.EntityConfiguration
{
    public class FilterConfiguration : IEntityTypeConfiguration<FilterEntity>
    {
        public void Configure(EntityTypeBuilder<FilterEntity> builder)
        {
            builder.ToTable(DbConstants.Tables.Filter, DbConstants.CatalogSchema);

            builder.HasKey(f => f.Id);

            builder.Property(f => f.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(f => f.Type)
                .IsRequired();

            builder.HasOne(f => f.Category)
                .WithMany(c => c.Filters)
                .HasForeignKey(f => f.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(f => f.CategoryId);
            builder.HasIndex(f => f.Name);
        }
    }
}
