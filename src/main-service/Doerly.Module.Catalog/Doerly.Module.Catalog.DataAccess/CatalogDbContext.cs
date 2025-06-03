using Doerly.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CategoryEntity = Doerly.Module.Catalog.DataAccess.Models.Category;
using FilterEntity = Doerly.Module.Catalog.DataAccess.Models.Filter;
using ServiceFilterValueEntity = Doerly.Module.Catalog.DataAccess.Models.ServiceFilterValue;
using ServiceEntity = Doerly.Module.Catalog.DataAccess.Models.Service;

namespace Doerly.Module.Catalog.DataAccess
{
    public class CatalogDbContext : BaseDbContext
    {
        public CatalogDbContext(IConfiguration configuration) : base(configuration)
        {
        }

        public DbSet<CategoryEntity> Categories { get; set; }

        public DbSet<FilterEntity> Filters { get; set; }

        public DbSet<ServiceFilterValueEntity> ServiceFilterValues { get; set; }

        public DbSet<ServiceEntity> Services { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Configuration["ConnectionStrings:CatalogConnection"]);

            base.OnConfiguring(optionsBuilder);
        }
    }
}
