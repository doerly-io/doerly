using Doerly.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProfileModel = Doerly.Module.Profile.DataAccess.Models.Profile;

namespace Doerly.Module.Profile.DataAccess;

public class ProfileDbContext(IConfiguration configuration) : BaseDbContext(configuration)
{
    public DbSet<ProfileModel> Profiles { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProfileDbContext).Assembly);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(Configuration["ConnectionStrings:ProfileConnection"]);

        base.OnConfiguring(optionsBuilder);
    }
    
}