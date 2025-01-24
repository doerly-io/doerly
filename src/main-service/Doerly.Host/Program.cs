using System.Globalization;
using Doerly.Host;
using System.Reflection;
using System.Text;
using Doerly.Domain.Factories;
using Doerly.Common;
using Doerly.Api.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

var mvcBuilder = builder.Services.AddControllers();

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
    moduleInitializer.ConfigureServices(builder, mvcBuilder);
}

#endregion


builder.Services.AddScoped<IHandlerFactory, HandlerFactory>();

var jwtSettingsConfiguration = configuration.GetSection(JwtSettings.JwtSettingsName);
var jwtSettings = jwtSettingsConfiguration.Get<JwtSettings>();
builder.Services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.JwtSettingsName));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
            ClockSkew = TimeSpan.FromSeconds(jwtSettings.AccessTokenExpiration),
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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
        new("en-US")
        {
            DateTimeFormat =
            {
                LongTimePattern = "MM/DD/YYYY",
                ShortTimePattern = "MM/DD/YYYY"
            }
        },
        new("uk-UA")
        {
            DateTimeFormat =
            {
                LongTimePattern = "DD/MM/YYYY",
                ShortTimePattern = "DD/MM/YYYY"
            }
        }
    };

    options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

app.UseRouting();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

var moduleInitializers = app.Services.GetServices<IModuleInitializer>();
foreach (var moduleInitializer in moduleInitializers)
{
    moduleInitializer.Configure(app, app.Environment);
}

app.Run();
