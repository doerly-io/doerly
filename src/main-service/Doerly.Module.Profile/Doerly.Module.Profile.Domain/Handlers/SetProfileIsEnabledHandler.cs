using Doerly.Domain.Models;
using Doerly.Module.Profile.DataAccess;
using Doerly.Module.Profile.DataTransferObjects;
using Doerly.Proxy.Authorization;

namespace Doerly.Module.Profile.Domain.Handlers;

public class SetProfileIsEnabledHandler(
    ProfileDbContext dbContext,
    IAuthorizationModuleProxy authorizationModuleProxy
    ) : BaseProfileHandler(dbContext)
{
    public async Task<OperationResult> HandleAsync(int userId, EnableUserDto dto)
    {
        var result = await authorizationModuleProxy.ChangeUserState(userId, dto.IsEnabled);
        
        if (!result.IsSuccess)
            return OperationResult.Failure(result.ErrorMessage);
        
        return OperationResult.Success();
    }
}
