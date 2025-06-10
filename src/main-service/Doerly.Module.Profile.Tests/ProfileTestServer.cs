using System.Security.Claims;
using System.Text.Encodings.Web;
using Doerly.Domain;
using Doerly.Domain.Factories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Doerly.Module.Profile.DataAccess;
using Doerly.Module.Profile.Domain.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Doerly.Module.Profile.Tests;

public class ProfileTestServer : IDisposable
{
    public readonly TestServer _server;
    public HttpClient Client { get; }

    public ProfileTestServer(string connectionString)
    {
        var builder = new WebHostBuilder()
            .UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                // Add configuration with the connection string
                var configuration = new ConfigurationBuilder()
                    .AddInMemoryCollection(new Dictionary<string, string>
                    {
                        ["ConnectionStrings:ProfileConnection"] = connectionString
                    })
                    .Build();
                
                services.AddSingleton<IConfiguration>(configuration);
                
                services.AddDbContext<ProfileDbContext>(options =>
                {
                    options.UseNpgsql(connectionString);
                    options.EnableSensitiveDataLogging();
                    options.EnableDetailedErrors();
                });

                services.AddScoped<IHandlerFactory, HandlerFactory>();
                services.AddScoped<IDoerlyRequestContext, DoerlyRequestContext>();

                // Add controllers
                services.AddControllers()
                    .AddApplicationPart(typeof(Api.Controllers.ProfileController).Assembly);

                // Add your handlers
                services.AddScoped<CreateProfileHandler>();
                services.AddScoped<UpdateProfileHandler>();
                services.AddScoped<GetProfileHandler>();
                services.AddScoped<DeleteProfileHandler>();
                services.AddScoped<GetAllShortProfilesHandler>();
                services.AddScoped<SearchProfilesHandler>();
                services.AddScoped<UploadProfileImageHandler>();
                services.AddScoped<DeleteProfileImageHandler>();
                services.AddScoped<UploadProfileCvHandler>();
                services.AddScoped<DeleteProfileCvHandler>();
                services.AddScoped<SetProfileIsEnabledHandler>();
                services.AddScoped<SelectUserPaymentsHistory>();
                
                // Configure API behavior
                services.Configure<ApiBehaviorOptions>(options =>
                {
                    options.SuppressModelStateInvalidFilter = false;
                });

                // Disable authentication and authorization for tests
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>("Test", options => { });
                
                services.AddAuthorization(options =>
                {
                    options.DefaultPolicy = new AuthorizationPolicyBuilder()
                        .RequireAssertion(_ => true) // Always allow
                        .Build();
                });
            })
            .Configure(app =>
            {
                app.UseRouting();
                app.UseAuthentication();
                app.UseAuthorization();
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            });

        _server = new TestServer(builder);
        Client = _server.CreateClient();
    }

    public void Dispose()
    {
        Client?.Dispose();
        _server?.Dispose();
    }
}

// Simple test authentication handler that always succeeds
public class TestAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public TestAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "TestUser"),
            new Claim(ClaimTypes.NameIdentifier, "123")
        };

        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "Test");

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
