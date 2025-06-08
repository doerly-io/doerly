using System.Globalization;
using System.Text;
using Doerly.Domain.Factories;
using Doerly.Common.Settings;
using Doerly.Domain;
using Doerly.FileRepository;
using Doerly.Host.Middlewares;
using Doerly.Infrastructure.Api;
using Doerly.Localization;
using Doerly.Messaging;
using Doerly.Notification.EmailSender;
using Doerly.Proxy.Authorization;
using Doerly.Proxy.BaseProxy;
using Doerly.Proxy.Catalog;
using Doerly.Proxy.Orders;
using Doerly.Proxy.Payment;
using Doerly.Proxy.Profile;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.Azure;
using Microsoft.IdentityModel.Tokens;
using SendGrid.Extensions.DependencyInjection;
using Doerly.Host.ExceptionHandlers;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services
    .AddControllers(options =>
    {
        options.ModelMetadataDetailsProviders.Add(new SystemTextJsonValidationMetadataProvider());
    })
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
        {
            var resourceManager = Resources.ResourceManager;
            return new DataAnnotationsStringLocalizer(resourceManager);
        };
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });

builder.RegisterModule(new Doerly.Module.Payments.Api.ModuleInitializer());
builder.RegisterModule(new Doerly.Module.Authorization.Api.ModuleInitializer());
builder.RegisterModule(new Doerly.Module.Profile.Api.ModuleInitializer());
builder.RegisterModule(new Doerly.Module.Communication.Api.ModuleInitializer());
builder.RegisterModule(new Doerly.Module.Order.Api.ModuleInitializer());
builder.RegisterModule(new Doerly.Module.Catalog.Api.ModuleInitializer());
builder.RegisterModule(new Doerly.Module.Common.Api.ModuleInitializer());
builder.RegisterModule(new Doerly.Module.Statistics.Api.ModuleInitializer());


#region ModuleProxies

builder.Services.AddProxy<IPaymentModuleProxy, PaymentModuleProxy>();
builder.Services.AddProxy<IProfileModuleProxy, ProfileModuleProxy>();
builder.Services.AddProxy<IAuthorizationModuleProxy, AuthorizationModuleProxy>();
builder.Services.AddProxy<IOrdersModuleProxy, OrdersModuleProxy>();
builder.Services.AddProxy<ICatalogModuleProxy, CatalogModuleProxy>();

#endregion

builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<WebhookUrlBuilder>();

builder.Services.AddScoped<SendEmailHandler>();

builder.Services.AddScoped<IHandlerFactory, HandlerFactory>();

builder.Services.AddScoped<IMessagePublisher, MessagePublisher>();

builder.Services.ConfigureHttpClientDefaults(httpClientBuilder =>
    httpClientBuilder.AddStandardResilienceHandler(options => { }));

#region Configure Settings

var frontendSettingsCfg = configuration.GetSection(FrontendSettings.FrontendSettingsName);
builder.Services.AddOptions<FrontendSettings>()
    .Bind(frontendSettingsCfg)
    .ValidateDataAnnotations()
    .ValidateOnStart();

var frontendSettings = frontendSettingsCfg.Get<FrontendSettings>();

var sendGridSettingsCfg = configuration.GetSection(SendGridSettings.SendGridSettingsName);
builder.Services.AddOptions<SendGridSettings>()
    .Bind(sendGridSettingsCfg)
    .ValidateDataAnnotations()
    .ValidateOnStart();
var sendGridSettings = sendGridSettingsCfg.Get<SendGridSettings>();


var authSettingsConfiguration = configuration.GetSection(AuthSettings.AuthSettingsName);
builder.Services.AddOptions<AuthSettings>()
    .Bind(authSettingsConfiguration)
    .ValidateDataAnnotations()
    .ValidateOnStart();

var authSettings = authSettingsConfiguration.Get<AuthSettings>();

var azureStorageSettingsConfiguration = configuration.GetSection(AzureStorageSettings.AzureStorageSettingName);
builder.Services.AddOptions<AzureStorageSettings>()
    .Bind(azureStorageSettingsConfiguration)
    .ValidateDataAnnotations()
    .ValidateOnStart();

var azureStorageSettings = azureStorageSettingsConfiguration.Get<AzureStorageSettings>();

var backendSettingsConfiguration = configuration.GetSection(BackendSettings.BackendSettingsName);
builder.Services.AddOptions<BackendSettings>()
    .Bind(backendSettingsConfiguration)
    .ValidateDataAnnotations()
    .ValidateOnStart();

var redisSettingsConfiguration = configuration.GetSection(RedisSettings.RedisSettingName);
builder.Services.AddOptions<RedisSettings>()
    .Bind(redisSettingsConfiguration)
    .ValidateDataAnnotations()
    .ValidateOnStart();

var redisSettings = redisSettingsConfiguration.Get<RedisSettings>();

#endregion

builder.Services.AddSignalR().AddStackExchangeRedis(redisSettings.ConnectionString);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = authSettings.Issuer,
            ValidAudience = authSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.SecretKey)),
            ClockSkew = TimeSpan.FromMinutes(authSettings.AccessTokenLifetime),
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    path.StartsWithSegments("/communicationhub"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddExceptionHandler<DoerlyExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddSendGrid(opt => { opt.ApiKey = sendGridSettings.ApiKey; });

builder.Services.AddAzureClients(factoryBuilder =>
{
    factoryBuilder.AddBlobServiceClient(azureStorageSettings.ConnectionString);
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisSettings.ConnectionString;
    options.InstanceName = RedisSettings.RedisInstanceName;
});

builder.Services.AddTransient<IFileRepository, FileRepository>();

builder.Services.AddMassTransit(cfg =>
{
    cfg.SetEndpointNameFormatter(new DefaultEndpointNameFormatter(includeNamespace: true));
    cfg.UsingInMemory((context, factoryConfigurator) => { factoryConfigurator.ConfigureEndpoints(context); });

    cfg.AddConfigureEndpointsCallback((name, configurator) =>
    {
        configurator.UseMessageRetry(r => r.Intervals(2_000, 20_000, 60_000, 300_000, 600_000));
    });
});

builder.Services.AddScoped<IDoerlyRequestContext, DoerlyRequestContext>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRequestLocalization(options =>
{
    var supportedCultures = new List<CultureInfo>
    {
        new(LocalizationConstants.EnUsCulture)
        {
            DateTimeFormat =
            {
                LongTimePattern = LocalizationConstants.EnUsCultureDateTimeFormat,
                ShortTimePattern = LocalizationConstants.EnUsCultureDateTimeFormat
            }
        },
        new(LocalizationConstants.UkUaCulture)
        {
            DateTimeFormat =
            {
                LongTimePattern = LocalizationConstants.UkUaCultureDateTimeFormat,
                ShortTimePattern = LocalizationConstants.UkUaCultureDateTimeFormat
            }
        }
    };

    options.DefaultRequestCulture =
        new RequestCulture(culture: LocalizationConstants.EnUsCulture, uiCulture: LocalizationConstants.EnUsCulture);
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

app.UseWebSockets();
app.UseRouting();

app.UseCors(policy => policy
    .WithOrigins(frontendSettings.FrontendUrls)
    .AllowCredentials()
    .AllowAnyHeader()
    .AllowAnyMethod());

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<RequestContextMiddleware>();
app.UseExceptionHandler(options => { });

app.MapControllers();

var moduleInitializers = app.Services.GetServices<IModuleInitializer>();
foreach (var moduleInitializer in moduleInitializers)
{
    moduleInitializer.Configure(app, app.Environment);
}

var endpointInitializers = app.Services.GetServices<ISignalrEndpointRouteInitializer>();
foreach (var initializer in endpointInitializers)
{
    initializer.ConfigureEndpoints(app);
}

app.Run();