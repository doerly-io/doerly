using Doerly.DataAccess.Utils;
using Doerly.Module.Profile.DataAccess;
using ProfileRatingJob;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddDbContext<ProfileDbContext>();

var host = builder.Build();

host.Services.MigrateDatabase<ProfileDbContext>();

host.Run();
