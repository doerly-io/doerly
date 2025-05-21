using Doerly.Module.Authorization.DataAccess;
using Microsoft.Extensions.Configuration;

namespace Doerly.Module.Authorization.Tests;

public class BaseAuthTests : IClassFixture<PostgresTestContainerFixture>
{
    protected AuthorizationDbContext DbContext { get; private set; }

    public BaseAuthTests(PostgresTestContainerFixture fixture)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["ConnectionStrings:AuthorizationConnection"] = fixture.ConnectionString
            })
            .Build();

        DbContext = new AuthorizationDbContext(configuration);
        DbContext.Database.EnsureCreated();
    }
    
    
}