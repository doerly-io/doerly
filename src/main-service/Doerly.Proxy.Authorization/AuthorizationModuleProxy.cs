using Doerly.DataTransferObjects.Pagination;
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
}