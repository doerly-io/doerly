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
        
        builder.Property(x => x.ConversationName).HasMaxLength(255).IsRequired(false);;

        builder.Property(x => x.InitiatorId).IsRequired();
        
        builder.Property(x => x.RecipientId).IsRequired();
        
        builder.HasIndex(x => new { x.InitiatorId, x.RecipientId })
            .IsUnique();
        
        builder.Property(x => x.LastMessageId);
        
        builder.HasMany(x => x.Messages)
            .WithOne(x => x.Conversation)
            .HasForeignKey(x => x.ConversationId);
    }
}