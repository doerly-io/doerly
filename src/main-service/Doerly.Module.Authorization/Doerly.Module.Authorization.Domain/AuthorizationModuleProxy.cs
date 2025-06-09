using Doerly.DataTransferObjects.Pagination;
using Doerly.Domain.Factories;
using Doerly.Domain.Models;
using Doerly.Module.Authorization.DataTransferObjects.Responses;
using Doerly.Module.Authorization.Domain.Handlers;
using Doerly.Proxy.Authorization;

namespace Doerly.Module.Authorization.Domain;

public class AuthorizationModuleProxy : IAuthorizationModuleProxy
{
    private readonly IHandlerFactory _handlerFactory;

    public AuthorizationModuleProxy(IHandlerFactory handlerFactory)
    {
        _handlerFactory = handlerFactory;
    }

    public async Task<BasePaginationResponse<UserItemResponse>> GetUsersWithPaginationAsync(
        GetEntitiesWithPaginationRequest paginationRequest)
    {
        var result = await _handlerFactory.Get<SelectUsersHandler>().HandleAsync(paginationRequest);
        return result;
    }

    public async Task<OperationResult> ChangeUserState(int userId, bool isEnabled)
    {
        var result = await _handlerFactory.Get<ChangeUserStateHandler>().HandleAsync(userId, isEnabled);
        return result;
    }
    
    public async Task<List<UserItemResponse>> GetUserInfoByIdsAsync(IEnumerable<int> userIds)
    {
        var result = await _handlerFactory.Get<GetUserInfoByIdsHandler>().HandleAsync(userIds);
        return result;
    }

    public async Task<UserActivityStatisticsDto> GetActivityUsersStatisticsAsync()
    {
        return await _handlerFactory.Get<GetActivityUsersStatisticsHandler>().HandleAsync();
    }
}
