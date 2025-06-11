using Doerly.Domain;
using Doerly.Domain.Extensions;
using Doerly.Domain.Factories;
using Doerly.Messaging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Doerly.Module.Authorization.DataAccess;
using Doerly.Module.Profile.DataAccess;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Doerly.Module.Authorization.Tests;

public class AuthTestServer : IDisposable
{
    public readonly TestServer Server;
    public HttpClient Client { get; }

    public AuthTestServer(string authorizationConnectionString, string profileConnectionString = null)
    {
        var builder = new WebHostBuilder()
            .UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                // Add configuration with the connection strings
                var configDict = new Dictionary<string, string>
                {
                    ["ConnectionStrings:AuthorizationConnection"] = authorizationConnectionString
                };
                if (!string.IsNullOrEmpty(profileConnectionString))
                {
                    configDict["ConnectionStrings:ProfileConnection"] = profileConnectionString;
                }
                var configuration = new ConfigurationBuilder()
                    .AddInMemoryCollection(configDict)
                    .Build();
                services.AddSingleton<IConfiguration>(configuration);

                services.AddDbContext<AuthorizationDbContext>();
                services.AddDbContext<ProfileDbContext>();

                services.AddScoped<IHandlerFactory, HandlerFactory>();
                services.AddScoped<IDoerlyRequestContext, DoerlyRequestContext>();

                // Add IMessagePublisher mock for tests
                services.AddSingleton<IMessagePublisher, MessagePublisher>();

                // Add controllers
                services.AddControllers()
                    .AddApplicationPart(typeof(Api.Controllers.AuthController).Assembly);

                // Add your handlers
                // services.AddScoped<Domain.Handlers.LoginHandler>();
                // services.AddScoped<Domain.Handlers.RegistrationHandler>();
                // services.AddScoped<Domain.Handlers.RefreshTokenHandler>();
                // services.AddScoped<Domain.Handlers.LogoutHandler>();
                // services.AddScoped<Domain.Handlers.ResetPasswordHandler>();
                // services.AddScoped<Domain.Handlers.EmailVerificationHandler>();
                
                services.RegisterHandlers(typeof(Doerly.Module.Authorization.Domain.IAssemblyMarker).Assembly);
                services.RegisterHandlers(typeof(Doerly.Module.Profile.Domain.IAssemblyMarker).Assembly);

                // Configure API behavior
                services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = false; });
                
                // Register event consumers as in ModuleInitializer
                services.RegisterEventConsumers(typeof(Doerly.Module.Authorization.Domain.IAssemblyMarker).Assembly);
                services.RegisterEventConsumers(typeof(Doerly.Module.Profile.Domain.IAssemblyMarker).Assembly);

                // Register MassTransit (in-memory for tests)
                services.AddMassTransit(cfg =>
                {
                    cfg.UsingInMemory((context, factoryConfigurator) =>
                    {
                        factoryConfigurator.ConfigureEndpoints(context);
                    });
                });
            })
            .Configure(app =>
            {
                app.UseRouting();
                app.UseAuthentication();
                app.UseAuthorization();
                app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            });

        Server = new TestServer(builder);
        Client = Server.CreateClient();
    }

    public void Dispose()
    {
        Client?.Dispose();
        Server?.Dispose();
    }
}

