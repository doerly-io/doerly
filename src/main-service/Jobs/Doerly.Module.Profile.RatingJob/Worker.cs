using Doerly.Module.Profile.DataAccess;
using Doerly.Module.Profile.DataAccess.Constants;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Doerly.Module.Profile.RatingJob;

public class Worker : BackgroundService
{
    private readonly ProfileDbContext _profileDbContext;
    private readonly ILogger<Worker> _logger;

    public Worker(ProfileDbContext profileDbContext,
        ILogger<Worker> logger)
    {
        _profileDbContext = profileDbContext;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTime.UtcNow);

                await ProcessProfileRating();
                await ProcessCompetenceRating();
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }

    private async Task ProcessProfileRating()
    {
        const string query = $"""
                                          UPDATE {DbConstants.ProfileSchema}.{DbConstants.Tables.Profile} p
                                          SET rating = r.avg_rating
                                          FROM (
                                               SELECT reviewee_user_id AS user_id
                                                    , AVG(rating)      AS avg_rating
                                               FROM {DbConstants.ProfileSchema}.{DbConstants.Tables.Feedback} f
                                               GROUP BY f.reviewee_user_id
                                               ) r
                                          WHERE p.user_id = r.user_id;
                              """;

        using (var connection = new NpgsqlConnection(_profileDbContext.Database.GetConnectionString()))
        {
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = query;
            await command.ExecuteNonQueryAsync();
            await connection.CloseAsync();
        }
    }

    private async Task ProcessCompetenceRating()
    {
        const string query = $"""
                                 UPDATE {DbConstants.ProfileSchema}.{DbConstants.Tables.Competence} pc
                                 SET rating = sub.avg_rating
                                 FROM (
                                      SELECT reviewee_user_id, category_id, AVG(f.rating) AS avg_rating
                                      FROM {DbConstants.ProfileSchema}.{DbConstants.Tables.Feedback} f
                                      GROUP BY f.reviewee_user_id, f.category_id
                                      ) sub
                                          JOIN {DbConstants.ProfileSchema}.{DbConstants.Tables.Profile} p ON p.user_id = sub.reviewee_user_id
                                 WHERE pc.category_id = sub.category_id
                                   AND pc.profile_id = p.id
                              """;

        using (var connection = new NpgsqlConnection(_profileDbContext.Database.GetConnectionString()))
        {
            await connection.OpenAsync();
            var command = connection.CreateCommand();
            command.CommandText = query;
            await command.ExecuteNonQueryAsync();
            await connection.CloseAsync();
        }
    }
}