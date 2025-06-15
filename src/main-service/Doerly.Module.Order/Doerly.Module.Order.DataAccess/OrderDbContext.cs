using Doerly.DataAccess;
using Doerly.Module.Order.DataAccess.Entities;
using OrderEntity = Doerly.Module.Order.DataAccess.Entities.Order;
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

    public DbSet<OrderFile> OrderFiles { get; set; }
    
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
