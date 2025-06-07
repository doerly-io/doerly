using Doerly.Domain.Models;
using Doerly.Module.Profile.Contracts.Dtos;
using Doerly.Module.Profile.DataAccess;
using Doerly.Proxy.Authorization;

namespace Doerly.Module.Profile.Domain.Handlers;

public class SetProfileIsEnabledHandler(
    ProfileDbContext dbContext,
    IAuthorizationModuleProxy authorizationModuleProxy
    ) : BaseProfileHandler(dbContext)
{
    public async Task<HandlerResult> HandleAsync(int userId, EnableUserDto dto)
    {
        var result = await authorizationModuleProxy.ChangeUserState(userId, dto.IsEnabled);
        
        if (!result.IsSuccess)
            return HandlerResult.Failure(result.ErrorMessage);
        
        return HandlerResult.Success();
    }
}