using Doerly.DataTransferObjects.Pagination;
using Doerly.Module.Authorization.Contracts.Responses;
using Doerly.Proxy.BaseProxy;

namespace Doerly.Proxy.Authorization;

public interface IAuthorizationModuleProxy : IModuleProxy
{
    Task<BasePaginationResponse<UserItemResponse>> GetUsersWithPaginationAsync(
        GetEntitiesWithPaginationRequest paginationRequest);
    
}