using Doerly.DataTransferObjects.Pagination;
using Doerly.Domain.Factories;
using Doerly.Domain.Models;
using Doerly.Module.Authorization.Contracts.Responses;
using Doerly.Module.Authorization.Domain.Handlers.Metrics;
using Doerly.Module.Authorization.Domain.Handlers.Users;

namespace Doerly.Module.Authorization.Api.ModuleWrapper;

public interface IAuthorizationModuleWrapper
{
    Task<BasePaginationResponse<UserItemResponse>> GetUsersWithPaginationAsync(
        GetEntitiesWithPaginationRequest paginationRequest);

    Task<HandlerResult> ChangeUserState(int userId, bool isEnabled);
    
    Task<List<UserItemResponse>> GetUserInfoByIdsAsync(IEnumerable<int> userIds);

    Task<UserActivityStatisticsDto> GetActivityUsersStatisticsAsync();
}

public class AuthorizationModuleWrapper : IAuthorizationModuleWrapper
{
    private readonly IHandlerFactory _handlerFactory;

    public AuthorizationModuleWrapper(IHandlerFactory handlerFactory)
    {
        _handlerFactory = handlerFactory;
    }

    public async Task<BasePaginationResponse<UserItemResponse>> GetUsersWithPaginationAsync(
        GetEntitiesWithPaginationRequest paginationRequest)
    {
        var result = await _handlerFactory.Get<SelectUsersHandler>().HandleAsync(paginationRequest);
        return result;
    }

    public async Task<HandlerResult> ChangeUserState(int userId, bool isEnabled)
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
        return await _handlerFactory.Get<GetActivityUsersStatistics>().HandleAsync();
    }
}