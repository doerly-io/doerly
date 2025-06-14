using Doerly.DataAccess;
using Doerly.Module.Notification.DataAccess.Constants;
using Doerly.Module.Notification.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Doerly.Module.Notification.DataAccess;

public class NotificationDbContext(IConfiguration configuration) : BaseDbContext(configuration)
{
    public DbSet<NotificationEntity> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NotificationDbContext).Assembly);
        modelBuilder.HasDefaultSchema(NotificationConstants.NotificationSchema);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(Configuration["ConnectionStrings:NotificationConnection"]);
        base.OnConfiguring(optionsBuilder);
    }
} 