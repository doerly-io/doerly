using System.Globalization;
using Doerly.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Doerly.DataAccess;

public abstract class BaseDbContext : DbContext, IDbContext
{
    protected readonly IConfiguration Configuration;

    protected BaseDbContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    #region SaveChanges

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        OnBeforeSaving();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override int SaveChanges()
    {
        OnBeforeSaving();
        return base.SaveChanges();
    }

    private void OnBeforeSaving()
    {
        var entries = ChangeTracker.Entries().Where(x =>
            x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var now = DateTime.UtcNow;
            if (entry.State == EntityState.Added)
            {
                ((BaseEntity)entry.Entity).DateCreated = now;
            }

            ((BaseEntity)entry.Entity).LastModifiedDate = now;
        }
    }

    #endregion

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (Configuration["Environment"] == "Development")
            optionsBuilder.EnableSensitiveDataLogging().LogTo(Console.WriteLine, LogLevel.Information);

        optionsBuilder.UseSnakeCaseNamingConvention(CultureInfo.InvariantCulture);

        base.OnConfiguring(optionsBuilder);
    }
}
