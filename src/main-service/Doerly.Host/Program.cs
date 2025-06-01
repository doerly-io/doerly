using System.Globalization;
using Doerly.Host;
using System.Reflection;
using System.Resources;
using System.Text;
using Doerly.Domain.Factories;
using Doerly.Api.Infrastructure;
using Doerly.Common.Settings;
using Doerly.FileRepository;
using Doerly.Localization;
using Doerly.Messaging;
using Doerly.Notification.EmailSender;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.Azure;
using Microsoft.IdentityModel.Tokens;
using SendGrid.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddControllers(options => { options.ModelMetadataDetailsProviders.Add(new SystemTextJsonValidationMetadataProvider()); })
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
        {
            var resourceManager = new ResourceManager("Doerly.Localization.Resources", typeof(Resources).Assembly);
            return new DataAnnotationsStringLocalizer(resourceManager);
        };
    })
    .AddJsonOptions(options => { options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase; });

#region Configure Modules

builder.RegisterModule(new Doerly.Module.Payments.Api.ModuleInitializer());
builder.RegisterModule(new Doerly.Module.Authorization.Api.ModuleInitializer());
builder.RegisterModule(new Doerly.Module.Profile.Api.ModuleInitializer());
builder.RegisterModule(new Doerly.Module.Order.Api.ModuleInitializer());
builder.RegisterModule(new Doerly.Module.Catalog.Api.ModuleInitializer());
builder.RegisterModule(new Doerly.Module.Common.Api.ModuleInitializer());

var loadedPaths = loadedAssemblies.Select(a => a.Location).ToArray();

var referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, $"{HostConstants.MODULE_PREFIX}.*.dll");
var toLoad = referencedPaths.Where(r => !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase));

foreach (var path in toLoad)
{
    Assembly.LoadFrom(path);
}

loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies()
    .Where(a => a.GetName().Name!.StartsWith(HostConstants.MODULE_PREFIX));

foreach (var moduleAssembly in loadedAssemblies)
{
    var moduleInitializerType = moduleAssembly.GetTypes()
        .FirstOrDefault(t => typeof(IModuleInitializer).IsAssignableFrom(t) && !t.IsAbstract);

    if (moduleInitializerType == null)
        continue;

    var moduleInitializer = (IModuleInitializer)Activator.CreateInstance(moduleInitializerType);
    if (moduleInitializer != null)
    {
        builder.Services.AddSingleton(moduleInitializer);
        moduleInitializer.ConfigureServices(builder);
    }
}

#endregion

builder.Services.AddScoped<SendEmailHandler>();

builder.Services.AddScoped<IHandlerFactory, HandlerFactory>();

builder.Services.AddScoped<IMessagePublisher, MessagePublisher>();

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

#endregion


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
    });

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSendGrid(opt => { opt.ApiKey = sendGridSettings.ApiKey; });

builder.Services.AddAzureClients(factoryBuilder => { factoryBuilder.AddBlobServiceClient(azureStorageSettings.ConnectionString); });

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

app.UseRouting();

app.UseCors(policy => policy
    .WithOrigins(frontendSettings.FrontendUrl)
    .AllowCredentials()
    .AllowAnyHeader()
    .AllowAnyMethod());

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

var moduleInitializers = app.Services.GetServices<IModuleInitializer>();
foreach (var moduleInitializer in moduleInitializers)
{
    moduleInitializer.Configure(app, app.Environment);
}

app.Run();
