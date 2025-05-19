using Doerly.Module.Catalog.DataAccess.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using CategoryEntity = Doerly.Module.Catalog.DataAccess.Models.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doerly.Module.Catalog.DataAccess.EntityConfiguration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<CategoryEntity>
    {
        public void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            builder.ToTable(DbConstants.Tables.Category, DbConstants.CatalogSchema);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Description).HasMaxLength(1000);
            builder.Property(x => x.IsDeleted).IsRequired();
            builder.Property(x => x.IsEnabled).IsRequired();

            builder.HasOne(x => x.Parent)
                   .WithMany(x => x.Children)
                   .HasForeignKey(x => x.ParentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Services)
                   .WithOne(x => x.Category)
                   .HasForeignKey(x => x.CategoryId);
        }
    }
}
