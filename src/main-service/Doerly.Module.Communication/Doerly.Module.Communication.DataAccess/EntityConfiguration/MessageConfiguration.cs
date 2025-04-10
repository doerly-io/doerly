using Doerly.Module.Communication.DataAccess.Constants;
using Doerly.Module.Communication.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doerly.Module.Communication.DataAccess.EntityConfiguration;

public class MessageConfiguration : IEntityTypeConfiguration<MessageEntity>
{
    public void Configure(EntityTypeBuilder<MessageEntity> builder)
    {
        builder.ToTable(DbConstants.Tables.Message, DbConstants.CommunicationSchema);
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.SenderId).IsRequired();
        
        builder.Property(x => x.ConversationId).IsRequired();
        builder.HasOne(x => x.Conversation)
            .WithMany(x => x.Messages)
            .HasForeignKey(x => x.ConversationId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(x => x.MessageType).IsRequired().HasConversion<string>();
        builder.Property(x => x.MessageContent).IsRequired();
        builder.Property(x => x.SentAt).IsRequired();
        builder.Property(x => x.Status).HasConversion<string>();
    }
}