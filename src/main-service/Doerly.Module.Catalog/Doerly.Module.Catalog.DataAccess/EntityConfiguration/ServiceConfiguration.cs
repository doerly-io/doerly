using Doerly.Module.Catalog.DataAccess.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ServiceEntity = Doerly.Module.Catalog.DataAccess.Models.Service;

namespace Doerly.Module.Catalog.DataAccess.EntityConfiguration
{
    public class ServiceConfiguration : IEntityTypeConfiguration<ServiceEntity>
    {
        public void Configure(EntityTypeBuilder<ServiceEntity> builder)
        {
            builder.ToTable(DbConstants.Tables.Service, DbConstants.CatalogSchema);

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(s => s.Description)
                .HasMaxLength(1000);

            builder.Property(s => s.Price)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(s => s.Category)
                .WithMany(c => c.Services)
                .HasForeignKey(s => s.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasIndex(s => s.CategoryId);
            builder.HasIndex(s => s.UserId);
            builder.HasIndex(s => s.IsDeleted);
            builder.HasIndex(s => s.IsEnabled);
            builder.HasIndex(s => s.Name);
        }
    }
}
