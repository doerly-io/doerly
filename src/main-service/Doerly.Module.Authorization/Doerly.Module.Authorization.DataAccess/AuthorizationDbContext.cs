using Doerly.DataAccess;
using Doerly.Module.Authorization.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Doerly.Module.Authorization.DataAccess;

public class AuthorizationDbContext : BaseDbContext
{
    public AuthorizationDbContext(IConfiguration configuration) : base(configuration)
    {
    }

    public DbSet<User> Users { get; set; }
    
    public DbSet<Role> Roles { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public DbSet<ResetToken> ResetTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthorizationDbContext).Assembly);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(Configuration["ConnectionStrings:AuthorizationConnection"]);

        base.OnConfiguring(optionsBuilder);
    }
}
