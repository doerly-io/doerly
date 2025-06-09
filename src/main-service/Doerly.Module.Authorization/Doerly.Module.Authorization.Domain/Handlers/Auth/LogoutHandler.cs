using System.Text;
using Doerly.Common.Settings;
using Doerly.Domain.Models;
using Doerly.Module.Authorization.DataAccess;
using Doerly.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Doerly.Module.Authorization.Domain.Handlers;

public class LogoutHandler : BaseAuthHandler
{
    public LogoutHandler(AuthorizationDbContext dbContext, IOptions<AuthSettings> authOptions) : base(dbContext, authOptions)
    {
    }

    public async Task<OperationResult> HandleAsync(string refreshToken)
    {
        var tokenBytes = Encoding.UTF8.GetBytes(refreshToken);
        var hashedToken = GetResetTokenHash(tokenBytes);
        var resetTokenEntity = await DbContext.Tokens.FirstOrDefaultAsync(x => x.Value == hashedToken);
        if (resetTokenEntity == null)
            return OperationResult.Failure(Resources.Get("UserNotFound"));

        DbContext.Tokens.Remove(resetTokenEntity);
        await DbContext.SaveChangesAsync();

        return OperationResult.Success();
    }
}
