using Doerly.Common;
using Doerly.Domain.Models;
using Doerly.Module.Authorization.DataAccess;
using Doerly.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Doerly.Module.Authorization.Domain.Handlers;

public class LogoutHandler : BaseAuthHandler
{
    public LogoutHandler(AuthorizationDbContext dbContext, IOptions<AuthSettings> jwtOptions) : base(dbContext, jwtOptions)
    {
    }
    
    public async Task<HandlerResult> HandleAsync(Guid refreshToken)
    {
        var refreshTokenEntity = await DbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Guid == refreshToken);
        if (refreshTokenEntity == null)
            return HandlerResult.Failure(Resources.Get("InvalidRefreshToken"));
        
        DbContext.RefreshTokens.Remove(refreshTokenEntity);
        await DbContext.SaveChangesAsync();
        
        return HandlerResult.Success();
    }
}
