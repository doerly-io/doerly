using OrderEntity = Doerly.Module.Order.DataAccess.Models.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doerly.Module.Order.DataAccess.EntityConfigurations;

public class OrderConfiguration : IEntityTypeConfiguration<OrderEntity>
{
    public void Configure(EntityTypeBuilder<OrderEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CategoryId).IsRequired();
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Description).IsRequired();
        builder.Property(x => x.Price).IsRequired();
        builder.Property(x => x.PaymentKind).IsRequired();
        builder.Property(x => x.DueDate).IsRequired();
        builder.Property(x => x.Status).IsRequired();
        builder.Property(x => x.CustomerId).IsRequired();

        builder.HasMany(x => x.ExecutionProposals).WithOne(x => x.Order).HasForeignKey(x => x.OrderId);
    }
}
