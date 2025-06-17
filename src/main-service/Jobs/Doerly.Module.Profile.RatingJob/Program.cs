using Doerly.Module.Profile.DataAccess;
using Doerly.Module.Profile.RatingJob;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddDbContext<ProfileDbContext>();

var host = builder.Build();
host.Run();