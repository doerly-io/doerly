using Doerly.Module.Catalog.DataAccess.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using CategoryEntity = Doerly.Module.Catalog.DataAccess.Models.Category;

namespace Doerly.Module.Catalog.DataAccess.EntityConfiguration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<CategoryEntity>
    {
        public void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            builder.ToTable(DbConstants.Tables.Category, DbConstants.CatalogSchema);

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Description)
                .HasMaxLength(500);

            builder.HasOne(c => c.Parent)
                .WithMany(c => c.Children)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(c => c.Name);
            builder.HasIndex(c => c.IsDeleted);
            builder.HasIndex(c => c.IsEnabled);
        }
    }
}
