using Doerly.Domain.Handlers;
using Doerly.Module.Authorization.Contracts.Responses;
using Doerly.Module.Authorization.DataAccess;
using Doerly.Module.Authorization.DataAccess.Entities;
using Doerly.Module.Authorization.Enums;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Authorization.Domain.Handlers.Metrics;

public class GetActivityUsersStatistics : BaseHandler<AuthorizationDbContext>
{
    public GetActivityUsersStatistics(AuthorizationDbContext dbContext) : base(dbContext)
    {
    }
    
    public async Task<UserActivityStatisticsDto> HandleAsync()
    {
        var utcNow = DateTime.UtcNow;
        var last24Hours = utcNow.AddHours(-24);
        var last7Days = utcNow.AddDays(-7);
        var last30Days = utcNow.AddDays(-30);
        
        // Get all refresh tokens (representing user login activities)
        var refreshTokens = await DbContext.Set<TokenEntity>()
            .Where(t => t.TokenKind == ETokenKind.RefreshToken)
            .Include(t => t.User)
            .ToListAsync();

        // Active users based on refresh token creation dates
        var activeUsersLast24Hours = refreshTokens
            .Where(t => t.DateCreated >= last24Hours)
            .Select(t => t.UserId)
            .Distinct()
            .Count();

        var activeUsersLast7Days = refreshTokens
            .Where(t => t.DateCreated >= last7Days)
            .Select(t => t.UserId)
            .Distinct()
            .Count();

        var activeUsersLast30Days = refreshTokens
            .Where(t => t.DateCreated >= last30Days)
            .Select(t => t.UserId)
            .Distinct()
            .Count();

        // Activity by hour for last 24 hours
        var activityByHourLast24Hours = new Dictionary<DateTime, int>();
        for (int i = 0; i < 24; i++)
        {
            var hourStart = utcNow.AddHours(-i).Date.AddHours(utcNow.AddHours(-i).Hour);
            var hourEnd = hourStart.AddHours(1);
            
            var count = refreshTokens
                .Where(t => t.DateCreated >= hourStart && t.DateCreated < hourEnd)
                .Select(t => t.UserId)
                .Distinct()
                .Count();
                
            activityByHourLast24Hours[hourStart] = count;
        }

        // Activity by day for last 7 days
        var activityByDayLast7Days = new Dictionary<DayOfWeek, int>();
        for (int i = 0; i < 7; i++)
        {
            var day = utcNow.AddDays(-i).Date;
            var dayOfWeek = day.DayOfWeek;
            
            var count = refreshTokens
                .Where(t => t.DateCreated.Date == day)
                .Select(t => t.UserId)
                .Distinct()
                .Count();
                
            if (activityByDayLast7Days.ContainsKey(dayOfWeek))
            {
                activityByDayLast7Days[dayOfWeek] += count;
            }
            else
            {
                activityByDayLast7Days[dayOfWeek] = count;
            }
        }

        // Total users count
        var totalUsersCount = await DbContext.Set<UserEntity>()
            .CountAsync();

        // Users active in last 30 days
        var activeUserIdsLast30Days = refreshTokens
            .Where(t => t.DateCreated >= last30Days)
            .Select(t => t.UserId)
            .Distinct()
            .ToHashSet();

        // Inactive users in last 30 days (total users - active users)
        var inactiveUsersLast30Days = totalUsersCount - activeUserIdsLast30Days.Count;

        // New users in last 30 days
        var newUsersLast30Days = await DbContext.Set<UserEntity>()
            .Where(u => u.DateCreated >= last30Days)
            .CountAsync();

        return new UserActivityStatisticsDto
        {
            ActiveUsersLast24Hours = activeUsersLast24Hours,
            ActiveUsersLast7Days = activeUsersLast7Days,
            ActiveUsersLast30Days = activeUsersLast30Days,
            ActivityByHourLast24Hours = activityByHourLast24Hours,
            ActivityByDayLast7Days = activityByDayLast7Days,
            TotalUsersCount = totalUsersCount,
            InactiveUsersLast30Days = inactiveUsersLast30Days,
            NewUsersLast30Days = newUsersLast30Days
        };

    }
}