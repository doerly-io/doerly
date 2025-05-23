using Doerly.Module.Profile.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace ProfileRatingJob;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ProfileDbContext _profileDbContext;

    public Worker(ILogger<Worker> logger, ProfileDbContext profileDbContext)
    {
        _logger = logger;
        _profileDbContext = profileDbContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var jobStartedAt = DateTime.UtcNow;
            _logger.LogInformation("Worker running at: {time}", jobStartedAt);

            await _profileDbContext.Profiles.ExecuteUpdateAsync(calls => calls
                    .SetProperty(profile => profile.Rating, profile => profile.ReviewsReceived.Average(r => r.Rating)),
                cancellationToken: stoppingToken);

            var jobFinishedAt = DateTime.UtcNow;
            var elapsed = jobFinishedAt - jobStartedAt;

            _logger.LogInformation("Worker finished running at: {time}, elapsed: {elapsed}", jobFinishedAt, elapsed);

            await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
        }
    }
}
