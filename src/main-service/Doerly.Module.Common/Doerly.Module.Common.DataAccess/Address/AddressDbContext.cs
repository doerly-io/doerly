using Doerly.DataAccess;
using Doerly.Module.Common.DataAccess.Address.Constants;
using Doerly.Module.Common.DataAccess.Address.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Doerly.Module.Common.DataAccess.Address;

public class AddressDbContext : BaseDbContext
{
    public AddressDbContext(IConfiguration configuration) : base(configuration)
    {
    }

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
