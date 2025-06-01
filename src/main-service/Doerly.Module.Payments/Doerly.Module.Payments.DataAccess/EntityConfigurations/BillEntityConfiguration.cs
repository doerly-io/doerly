using Doerly.Module.Payments.DataAccess.Constants;
using Doerly.Module.Payments.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doerly.Module.Payments.DataAccess.EntityConfigurations;

public class BillEntityConfiguration : IEntityTypeConfiguration<Bill>
{
    public void Configure(EntityTypeBuilder<Bill> builder)
    {
        builder.ToTable(DbConstants.Tables.Bill, DbConstants.PaymentSchema);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.PayerId).IsRequired();
        builder.Property(x => x.AmountTotal).HasPrecision(18, 2);
        builder.Property(x => x.AmountPaid).HasPrecision(18, 2);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(200);
        builder.HasMany(x => x.Payments).WithOne(x => x.Bill).HasForeignKey(x => x.BillId);
    }
}
