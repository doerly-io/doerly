using Doerly.Module.Profile.DataAccess;
using Microsoft.Extensions.Configuration;

namespace Doerly.Module.Profile.Tests;

public class BaseProfileTests : IClassFixture<PostgresTestContainerFixture>
{
    protected ProfileDbContext DbContext { get; private set; }

    public BaseProfileTests(PostgresTestContainerFixture fixture)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["ConnectionStrings:ProfileConnection"] = fixture.ConnectionString
            })
            .Build();

        DbContext = new ProfileDbContext(configuration);
        DbContext.Database.EnsureCreated();
    }
}
