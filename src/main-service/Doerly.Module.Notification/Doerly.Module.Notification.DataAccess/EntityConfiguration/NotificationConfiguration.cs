using Doerly.Module.Notification.DataAccess.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Doerly.Module.Notification.DataAccess.Entities;

namespace Doerly.Module.Notification.DataAccess.EntityConfiguration;

public class NotificationConfiguration : IEntityTypeConfiguration<NotificationEntity>
{
    public void Configure(EntityTypeBuilder<NotificationEntity> builder)
    {
        builder.ToTable(NotificationConstants.Tables.Notification, NotificationConstants.NotificationSchema);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Message).IsRequired().HasMaxLength(1000);
        builder.Property(x => x.Type).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Data).HasMaxLength(2000);
        builder.Property(x => x.IsRead).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();

        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.CreatedAt);
        builder.HasIndex(x => new { x.UserId, x.IsRead });
    }
} 