using OrderEntity = Doerly.Module.Order.DataAccess.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Doerly.Module.Order.DataAccess.Constants;

namespace Doerly.Module.Order.DataAccess.EntityConfigurations;

public class OrderConfiguration : IEntityTypeConfiguration<OrderEntity>
{
    public void Configure(EntityTypeBuilder<OrderEntity> builder)
    {
        builder.ToTable(DbConstants.Tables.Order, DbConstants.OrderSchema);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CategoryId).IsRequired();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(4000);
        builder.Property(x => x.Price).IsRequired().HasPrecision(15, 2);
        builder.Property(x => x.PaymentKind).IsRequired();
        builder.Property(x => x.DueDate).IsRequired();
        builder.Property(x => x.Status).IsRequired();
        builder.Property(x => x.CustomerId).IsRequired();
        builder.Property(x => x.Code).IsRequired().HasMaxLength(36);
        builder.Property(x => x.RegionId).IsRequired();
        builder.Property(x => x.CityId).IsRequired();
        builder.Property(x => x.UseProfileAddress).IsRequired();

        builder.HasMany(x => x.ExecutionProposals).WithOne(x => x.Order).HasForeignKey(x => x.OrderId);
        builder.HasMany(x => x.OrderFiles).WithOne(x => x.Order).HasForeignKey(x => x.OrderId);
    }
}
