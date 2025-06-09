using Doerly.DataTransferObjects.Pagination;
using Doerly.Domain.Models;
using Doerly.Module.Authorization.DataTransferObjects.Responses;

namespace Doerly.Module.Authorization.Contracts;

public interface IAuthorizationModuleWrapper
{
    Task<BasePaginationResponse<UserItemResponse>> GetUsersWithPaginationAsync(
        GetEntitiesWithPaginationRequest paginationRequest);

    Task<OperationResult> ChangeUserState(int userId, bool isEnabled);
    
    Task<List<UserItemResponse>> GetUserInfoByIdsAsync(IEnumerable<int> userIds);

    Task<UserActivityStatisticsDto> GetActivityUsersStatisticsAsync();
}
