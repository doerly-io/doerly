using Doerly.DataTransferObjects.Pagination;
using Doerly.Domain.Models;
using Doerly.Module.Authorization.DataTransferObjects.Responses;
using Doerly.Proxy.BaseProxy;

namespace Doerly.Proxy.Authorization;

public interface IAuthorizationModuleProxy : IModuleProxy
{
    Task<BasePaginationResponse<UserItemResponse>> GetUsersWithPaginationAsync(
        GetEntitiesWithPaginationRequest paginationRequest);
    
    Task<OperationResult> ChangeUserState(int userId, bool isEnabled);
    
    Task<List<UserItemResponse>> GetUserInfoByIdsAsync(IEnumerable<int> userIds);
    
    Task<UserActivityStatisticsDto> GetActivityUsersStatisticsAsync();
}