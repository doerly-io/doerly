using Doerly.DataAccess;
using Doerly.Module.Communication.DataAccess.Constants;
using Doerly.Module.Communication.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Doerly.Module.Communication.DataAccess;

public class CommunicationDbContext(IConfiguration configuration) : BaseDbContext(configuration)
{
    public DbSet<MessageEntity> Messages { get; set; }
    public DbSet<ConversationEntity> Conversations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CommunicationDbContext).Assembly);
        modelBuilder.HasDefaultSchema(DbConstants.CommunicationSchema);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(Configuration["ConnectionStrings:CommunicationConnection"]);
        base.OnConfiguring(optionsBuilder);
    }
}
