using Doerly.DataAccess.Constants;
using Doerly.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Doerly.DataAccess;

public class AddressDbContext(IConfiguration configuration) : BaseDbContext(configuration)
{
    public DbSet<City> Cities { get; set; }
    public DbSet<Region> Regions { get; set; }
    public DbSet<Street> Streets { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AddressDbContext).Assembly);
        modelBuilder.HasDefaultSchema(AddressDbConstants.AddressSchema);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(Configuration["ConnectionStrings:AddressConnection"]);

        base.OnConfiguring(optionsBuilder);
    }
}