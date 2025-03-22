using Doerly.Module.Communication.DataAccess.Constants;
using Doerly.Module.Communication.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doerly.Module.Communication.DataAccess.EntityConfiguration;

public class ConversationConfiguration : IEntityTypeConfiguration<ConversationEntity>
{
    public void Configure(EntityTypeBuilder<ConversationEntity> builder)
    {
        builder.ToTable(DbConstants.Tables.Conversation, DbConstants.CommunicationSchema);
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.ConversationName).HasMaxLength(255);
        
        builder.Property(x => x.InitiatorId);
        builder.HasOne(x => x.Initiator).WithMany().HasForeignKey(x => x.InitiatorId);
        
        builder.Property(x => x.RecipientId);
        builder.HasOne(x => x.Recipient).WithMany().HasForeignKey(x => x.RecipientId);
        
        builder.Property(x => x.LastMessageId);
        builder.HasMany(x => x.Messages).WithOne(x => x.Conversation).HasForeignKey(x => x.ConversationId);
    }
}