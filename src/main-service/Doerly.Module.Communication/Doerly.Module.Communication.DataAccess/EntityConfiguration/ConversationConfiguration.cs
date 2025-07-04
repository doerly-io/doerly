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
        builder.Property(x => x.InitiatorId).IsRequired();
        builder.Property(x => x.RecipientId).IsRequired();
        builder.Property(x => x.LastMessageId);
        
        builder.HasMany(x => x.Messages)
            .WithOne(x => x.Conversation)
            .HasForeignKey(x => x.ConversationId);
        
        builder.HasIndex(x => new { x.InitiatorId, x.RecipientId })
            .IsUnique();
    }
}
