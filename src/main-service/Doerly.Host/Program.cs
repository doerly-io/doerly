using System.Globalization;
using Doerly.Host;
using System.Reflection;
using System.Text;
using Doerly.Domain.Factories;
using Doerly.Common;
using Doerly.Api.Infrastructure;
using Doerly.Notification.EmailSender;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.IdentityModel.Tokens;
using SendGrid.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddLocalization(x => x.ResourcesPath = "Resources");
builder.Services.AddControllers()
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(Doerly.Localization.Resources));
    });

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

var frontendSettingsCfg = configuration.GetSection(FrontendSettings.FrontendSettingsName);
var frontendSettings = frontendSettingsCfg.Get<FrontendSettings>();
builder.Services.Configure<FrontendSettings>(configuration.GetSection(FrontendSettings.FrontendSettingsName));

var sendGridSettingsCfg = configuration.GetSection(SendGridSettings.SendGridSettingsName);
var sendGridSettings = sendGridSettingsCfg.Get<SendGridSettings>();
builder.Services.Configure<SendGridSettings>(configuration.GetSection(SendGridSettings.SendGridSettingsName));

var authSettingsConfiguration = configuration.GetSection(AuthSettings.AuthSettingsName);
var authSettings = authSettingsConfiguration.Get<AuthSettings>();
builder.Services.Configure<AuthSettings>(configuration.GetSection(AuthSettings.AuthSettingsName));

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
