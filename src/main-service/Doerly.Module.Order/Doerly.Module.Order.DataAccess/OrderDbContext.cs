using Doerly.DataAccess;
using Doerly.Module.Order.DataAccess.Models;
using OrderEntity = Doerly.Module.Order.DataAccess.Models.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Doerly.Module.Order.DataAccess;

public class OrderDbContext: BaseDbContext
{
    public OrderDbContext(IConfiguration configuration) : base(configuration)
    {
    }

    public DbSet<OrderEntity> Orders { get; set; }

    public DbSet<ExecutionProposal> ExecutionProposals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(Configuration["ConnectionStrings:OrderConnection"]);

        base.OnConfiguring(optionsBuilder);
    }
}
