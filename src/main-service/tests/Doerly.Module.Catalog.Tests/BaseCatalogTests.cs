using Doerly.Module.Catalog.DataAccess;
using Doerly.Module.Catalog.DataAccess.Models;
using Doerly.Module.Catalog.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Doerly.Module.Catalog.Tests;

public class BaseCatalogTests : IClassFixture<PostgresTestContainerFixture>
{
    protected CatalogDbContext CatalogDbContext { get; private set; }

    public BaseCatalogTests(PostgresTestContainerFixture fixture)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["ConnectionStrings:CatalogConnection"] = fixture.ConnectionString
            })
            .Build();

        CatalogDbContext = new CatalogDbContext(configuration);
        CatalogDbContext.Database.Migrate();
    }

    public async Task<Service> AddTestServiceAsync()
    {
        var category = new Category { Name = "Test Category" };
        CatalogDbContext.Categories.Add(category);
        await CatalogDbContext.SaveChangesAsync();

        var filter = new Filter { Name = "Color", Type = EFilterType.Checkbox };
        CatalogDbContext.Filters.Add(filter);
        await CatalogDbContext.SaveChangesAsync();

        var service = new Service
        {
            Name = "Test Service",
            Description = "Test Description",
            Price = 100,
            CategoryId = category.Id,
            UserId = 100,
            IsEnabled = true,
            IsDeleted = false,
            FilterValues = new List<ServiceFilterValue>
            {
                new ServiceFilterValue
                {
                    FilterId = filter.Id,
                    Value = "Red"
                }
            }
        };

        CatalogDbContext.Services.Add(service);
        await CatalogDbContext.SaveChangesAsync();

        return service;
    }
}
