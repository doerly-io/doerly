using Doerly.DataAccess;
using Doerly.Module.Authorization.DataAccess.Constants;
using Doerly.Module.Authorization.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Doerly.Module.Authorization.DataAccess;

public class AuthorizationDbContext : BaseDbContext
{
    public AuthorizationDbContext(IConfiguration configuration) : base(configuration)
    {
    }

    public DbSet<UserEntity> Users { get; set; }
    
    public DbSet<RoleEntity> Roles { get; set; }
    
    public DbSet<TokenEntity> Tokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthorizationDbContext).Assembly);
        modelBuilder.HasDefaultSchema(DbConstants.AuthSchema);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(Configuration["ConnectionStrings:AuthorizationConnection"]);
        
        base.OnConfiguring(optionsBuilder);
    }
}
