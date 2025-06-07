using Doerly.DataTransferObjects.Pagination;
using Doerly.Domain.Models;
using Doerly.Module.Authorization.Api.ModuleWrapper;
using Doerly.Module.Authorization.Contracts.Responses;

namespace Doerly.Proxy.Authorization;

public class AuthorizationModuleProxy : IAuthorizationModuleProxy
{
    private readonly IAuthorizationModuleWrapper _authorizationModuleWrapper;

    public AuthorizationModuleProxy(IAuthorizationModuleWrapper authorizationModuleWrapper)
    {
        _authorizationModuleWrapper = authorizationModuleWrapper;
    }

    public async Task<BasePaginationResponse<UserItemResponse>> GetUsersWithPaginationAsync(
        GetEntitiesWithPaginationRequest paginationRequest)
    {
        return await _authorizationModuleWrapper.GetUsersWithPaginationAsync(paginationRequest);
    }

    public async Task<HandlerResult> ChangeUserState(int userId, bool isEnabled)
    {
        return await _authorizationModuleWrapper.ChangeUserState(userId, isEnabled);
    }
    
    public async Task<List<UserItemResponse>> GetUserInfoByIdsAsync(IEnumerable<int> userIds)
    {
        return await _authorizationModuleWrapper.GetUserInfoByIdsAsync(userIds);
    }

    public async Task<UserActivityStatisticsDto> GetActivityUsersStatisticsAsync()
    {
        return await _authorizationModuleWrapper.GetActivityUsersStatisticsAsync();
    }
}