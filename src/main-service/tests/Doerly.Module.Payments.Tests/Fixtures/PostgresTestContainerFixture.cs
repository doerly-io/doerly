using DotNet.Testcontainers;
using Testcontainers.MsSql;
using Testcontainers.PostgreSql;

namespace Doerly.Module.Authorization.Tests;

public class PostgresTestContainerFixture : IAsyncLifetime
{
    public PostgreSqlContainer Container { get; private set; } = null!;
    public string ConnectionString => Container.GetConnectionString();

    public async ValueTask InitializeAsync()
    {
       Container = new PostgreSqlBuilder()
           .WithCleanUp(true)
           .WithImage("postgres:15")
           .WithDatabase("testdb")
           .WithUsername("testuser")
           .WithPassword("testpass")
           .WithEnvironment("POSTGRES_LOGGING_COLLECTOR", "on")
           .WithEnvironment("POSTGRES_LOG_STATEMENT", "all")
           .Build();

        await Container.StartAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await Container.StopAsync();
    }
}

public class MsSqlTestContainerFixture : IAsyncLifetime
{
    public MsSqlContainer Container { get; private set; } = null!;
    public string ConnectionString => Container.GetConnectionString();

    public async ValueTask InitializeAsync()
    {
        Container = new MsSqlBuilder()
            .WithCleanUp(true)
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            //.WithEnvironment("MSSQL_DATABASE", "testdb")

            .WithPassword("TestPass123!")
            .WithEnvironment("ACCEPT_EULA", "Y")
            .Build();
        ;
        await Container.StartAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await Container.StopAsync();
    }
}
