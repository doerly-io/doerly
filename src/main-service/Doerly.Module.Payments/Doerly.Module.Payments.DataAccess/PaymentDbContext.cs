using Doerly.DataAccess;
using Doerly.Module.Payments.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Doerly.Module.Payments.DataAccess;

public class PaymentDbContext : BaseDbContext
{
    public PaymentDbContext(IConfiguration configuration) : base(configuration)
    {
    }

    public DbSet<Bill> Bills { get; set; }

    public DbSet<Payment> Payments { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PaymentDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(Configuration["ConnectionStrings:PaymentConnection"]);

        base.OnConfiguring(optionsBuilder);
    }
}
