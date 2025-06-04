using Doerly.Module.Order.DataAccess.Constants;
using Doerly.Module.Order.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doerly.Module.Order.DataAccess.EntityConfigurations;

public class ExecutionProposalConfiguration : IEntityTypeConfiguration<ExecutionProposal>
{
    public void Configure(EntityTypeBuilder<ExecutionProposal> builder)
    {
        builder.ToTable(DbConstants.Tables.ExecutionProposal, DbConstants.OrderSchema);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.SenderId).IsRequired();
        builder.HasIndex(x => x.SenderId);
        builder.Property(x => x.ReceiverId).IsRequired();
        builder.HasIndex(x => x.ReceiverId);
        builder.Property(x => x.Status).IsRequired();
        builder.Property(x => x.Comment).IsRequired().HasMaxLength(1000);

        builder.HasOne(x => x.Order).WithMany(x => x.ExecutionProposals).HasForeignKey(x => x.OrderId);
    }
}
