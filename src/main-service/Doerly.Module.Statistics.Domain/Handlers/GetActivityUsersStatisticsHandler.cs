using Doerly.Domain.Handlers;
using Doerly.Domain.Models;
using Doerly.Module.Statistics.Contracts.Dtos;
using Doerly.Proxy.Authorization;
using MassTransit.Initializers;

namespace Doerly.Module.Statistics.Domain.Handlers;

public class GetActivityUsersStatisticsHandler : BaseHandler 
{
    private readonly IAuthorizationModuleProxy _authorizationModuleProxy;
    
    public GetActivityUsersStatisticsHandler(IAuthorizationModuleProxy authorizationModuleProxy)
    {
        _authorizationModuleProxy = authorizationModuleProxy;
    }

    public async Task<HandlerResult<UserActivityStatisticsDto>> HandleAsync()
    {
        var statistics = await _authorizationModuleProxy
            .GetActivityUsersStatisticsAsync()
            .Select(x => new UserActivityStatisticsDto
            {
                ActiveUsersLast24Hours = x.ActiveUsersLast24Hours,
                ActiveUsersLast7Days = x.ActiveUsersLast7Days,
                ActiveUsersLast30Days = x.ActiveUsersLast30Days,
                ActivityByHourLast24Hours = x.ActivityByHourLast24Hours,
                ActivityByDayLast7Days = x.ActivityByDayLast7Days,
                TotalUsersCount = x.TotalUsersCount,
                InactiveUsersLast30Days = x.InactiveUsersLast30Days,
                NewUsersLast30Days = x.NewUsersLast30Days
            });

        return HandlerResult.Success(statistics);
    }
}