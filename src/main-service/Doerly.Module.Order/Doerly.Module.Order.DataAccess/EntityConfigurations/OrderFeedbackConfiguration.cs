using Doerly.Module.Order.DataAccess.Constants;
using Doerly.Module.Order.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doerly.Module.Order.DataAccess.EntityConfigurations;

public class OrderFeedbackConfiguration : IEntityTypeConfiguration<OrderFeedback>
{
    public void Configure(EntityTypeBuilder<OrderFeedback> builder)
    {
        builder.ToTable(DbConstants.Tables.OrderFeedback, DbConstants.OrderSchema);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Comment).HasMaxLength(2000);
        builder.Property(x => x.Rating).IsRequired();

        builder.Property(x => x.OrderId).IsRequired();
        builder.HasOne(x => x.Order)
            .WithOne(x => x.Feedback)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.ToTable(t =>
            t.HasCheckConstraint("ck_feedback_rating_range", "\"rating\" >= 1 AND \"rating\" <= 5"));
    }
}
