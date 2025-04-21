using System.Globalization;
using Doerly.Host;
using System.Reflection;
using System.Resources;
using System.Text;
using Azure.Identity;
using Doerly.Domain.Factories;
using Doerly.Common;
using Doerly.Api.Infrastructure;
using Doerly.FileRepository;
using Doerly.Localization;
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

var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies()
    .Where(a => a.GetName().Name!.StartsWith(HostConstants.MODULE_PREFIX));

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
    builder.Services.AddSingleton(moduleInitializer);
    moduleInitializer.ConfigureServices(builder);
}

#endregion

builder.Services.AddScoped<SendEmailHandler>();

builder.Services.AddScoped<IHandlerFactory, HandlerFactory>();

#region Configure Settings

var frontendSettingsCfg = configuration.GetSection(FrontendSettings.FrontendSettingsName);
var frontendSettings = frontendSettingsCfg.Get<FrontendSettings>();
builder.Services.Configure<FrontendSettings>(frontendSettingsCfg);

var sendGridSettingsCfg = configuration.GetSection(SendGridSettings.SendGridSettingsName);
var sendGridSettings = sendGridSettingsCfg.Get<SendGridSettings>();
builder.Services.Configure<SendGridSettings>(sendGridSettingsCfg);

var authSettingsConfiguration = configuration.GetSection(AuthSettings.AuthSettingsName);
var authSettings = authSettingsConfiguration.Get<AuthSettings>();
builder.Services.Configure<AuthSettings>(authSettingsConfiguration);

var azureStorageSettingsConfiguration = configuration.GetSection(AzureStorageSettings.AzureStorageSettingName);
var azureStorageSettings = azureStorageSettingsConfiguration.Get<AzureStorageSettings>();
builder.Services.Configure<AzureStorageSettings>(azureStorageSettingsConfiguration);

#endregion

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.IncludeErrorDetails = true;
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

builder.Services.AddAzureClients(factoryBuilder =>
{
    factoryBuilder.AddBlobServiceClient(azureStorageSettings.ConnectionString);
});

builder.Services.AddTransient<IFileRepository, FileRepository>();

builder.Services.AddMassTransit(cfg =>
{
    cfg.SetEndpointNameFormatter(new DefaultEndpointNameFormatter(includeNamespace: true));
    cfg.UsingInMemory((context, factoryConfigurator) => { factoryConfigurator.ConfigureEndpoints(context); });

    cfg.AddConfigureEndpointsCallback((name, configurator) =>
    {
        configurator.UseMessageRetry(r => r.Exponential(
                5,
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(300),
                TimeSpan.FromSeconds(10)
            )
        );
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
        new(HostConstants.EnUsCulture)
        {
            DateTimeFormat =
            {
                LongTimePattern = "MM/DD/YYYY",
                ShortTimePattern = "MM/DD/YYYY"
            }
        },
        new(HostConstants.UkUaCulture)
        {
            DateTimeFormat =
            {
                LongTimePattern = "DD/MM/YYYY",
                ShortTimePattern = "DD/MM/YYYY"
            }
        }
    };

    options.DefaultRequestCulture = new RequestCulture(culture: HostConstants.EnUsCulture, uiCulture: HostConstants.EnUsCulture);
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
