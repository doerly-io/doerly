using System.Net;
using System.Net.Http.Json;
using Doerly.Module.Authorization.DataAccess;
using Doerly.Module.Authorization.Tests.Fixtures;
using Doerly.Module.Profile.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Doerly.Module.Authorization.Tests.E2E;

public class RegistrationEndToEndTests : IClassFixture<AuthAndProfilePostgresTestContainerFixture>
{
    private readonly string _profileConnectionString;
    private readonly HttpClient _authClient;
    private readonly ProfileDbContext _profileDbContext;
    private readonly AuthTestServer _authTestServer;
    private readonly string _authConnectionString;
    private readonly AuthorizationDbContext _authDbContext;

    public RegistrationEndToEndTests(AuthAndProfilePostgresTestContainerFixture fixture)
    {
        _profileConnectionString = fixture.ProfileConnectionString;
        _authConnectionString = fixture.AuthConnectionString;
        _authTestServer = new AuthTestServer(_authConnectionString, _profileConnectionString);
        _authClient = _authTestServer.Client;
        _profileDbContext = new ProfileDbContext(new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["ConnectionStrings:ProfileConnection"] = _profileConnectionString
            })
            .Build());
        _profileDbContext.Database.EnsureCreated();
        
        _authDbContext = new AuthorizationDbContext(new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["ConnectionStrings:AuthorizationConnection"] = _authConnectionString
            })
            .Build());
        _authDbContext.Database.EnsureCreated();
    }

    [Fact]
    public async Task RegisterUser_ShouldCreateProfileInProfileDb()
    {
        // Arrange
        var registerDto = new
        {
            Email = "e2euser1@test.com",
            Password = "TestPassword123!",
            FirstName = "E2E",
            LastName = "User1"
        };

        // Act
        var response = await _authClient.PostAsJsonAsync("/api/auth/register", registerDto);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        // Wait for eventual consistency if needed (e.g., if profile creation is async)
        await Task.Delay(5000);
        
        var profile = await _profileDbContext.Profiles
            .FirstOrDefaultAsync(p => p.FirstName == registerDto.FirstName 
                                      && p.LastName == registerDto.LastName);
        Assert.NotNull(profile);
        Assert.Equal(registerDto.FirstName, profile.FirstName);
        Assert.Equal(registerDto.LastName, profile.LastName);
    }

    [Fact]
    public async Task RegisterDuplicateUser_ShouldNotCreateDuplicateProfile()
    {
        // Arrange
        var registerDto = new
        {
            Email = "e2euser2@test.com",
            Password = "TestPassword123!",
            FirstName = "E2E",
            LastName = "User2"
        };

        // Act 1: Register first time
        var response1 = await _authClient.PostAsJsonAsync("/api/auth/register", registerDto);
        Assert.Equal(HttpStatusCode.Created, response1.StatusCode);

        // Act 2: Register duplicate
        var response2 = await _authClient.PostAsJsonAsync("/api/auth/register", registerDto);
        Assert.Equal(HttpStatusCode.Conflict, response2.StatusCode);

        // Assert: Only one profile exists
        var profiles = await _profileDbContext.Profiles.Where(p => p.FirstName == registerDto.FirstName && p.LastName == registerDto.LastName).ToListAsync();
        Assert.Single(profiles);
    }
}
