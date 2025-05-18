using Doerly.Module.Payments.DataAccess.Constants;
using Doerly.Module.Payments.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doerly.Module.Payments.DataAccess.EntityConfigurations;

public class PaymentEntityConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable(DbConstants.Tables.Payment, DbConstants.PaymentSchema);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Amount).HasPrecision(18, 2);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Currency).IsRequired().HasConversion<byte>();
        builder.Property(x => x.Status).IsRequired().HasConversion<byte>();
        builder.HasOne(x => x.Bill).WithMany(x => x.Payments).HasForeignKey(x => x.BillId);
    }
}
