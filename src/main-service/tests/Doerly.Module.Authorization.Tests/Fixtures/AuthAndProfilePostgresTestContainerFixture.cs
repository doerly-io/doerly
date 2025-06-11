using Testcontainers.PostgreSql;

namespace Doerly.Module.Authorization.Tests.Fixtures;

public class AuthAndProfilePostgresTestContainerFixture : IAsyncLifetime
{
    public PostgreSqlContainer AuthContainer { get; private set; } = null!;
    public PostgreSqlContainer ProfileContainer { get; private set; } = null!;

    public string AuthConnectionString => AuthContainer.GetConnectionString();
    public string ProfileConnectionString => ProfileContainer.GetConnectionString();

    public async ValueTask InitializeAsync()
    {
        AuthContainer = new PostgreSqlBuilder()
            .WithCleanUp(true)
            .WithImage("postgres:15")
            .WithDatabase("authdb")
            .WithDatabase("profiledb")
            .WithUsername("authuser")
            .WithPassword("authpass")
            .Build();

        ProfileContainer = new PostgreSqlBuilder()
            .WithCleanUp(true)
            .WithImage("postgres:15")
            .WithDatabase("profiledb")
            .WithUsername("profileuser")
            .WithPassword("profilepass")
            .Build();

        await AuthContainer.StartAsync();
        await ProfileContainer.StartAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await AuthContainer.StopAsync();
        await ProfileContainer.StopAsync();
    }
}

