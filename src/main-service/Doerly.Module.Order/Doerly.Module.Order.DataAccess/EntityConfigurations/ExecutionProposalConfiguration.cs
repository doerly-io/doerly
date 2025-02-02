using Doerly.Module.Order.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doerly.Module.Order.DataAccess.EntityConfigurations;

public class ExecutionProposalConfiguration : IEntityTypeConfiguration<ExecutionProposal>
{
    public void Configure(EntityTypeBuilder<ExecutionProposal> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.SenderId).IsRequired();
        builder.Property(x => x.ReceiverId).IsRequired();
        builder.Property(x => x.Status).IsRequired();

        builder.HasOne(x => x.Order).WithMany(x => x.ExecutionProposals).HasForeignKey(x => x.OrderId);
    }
}
