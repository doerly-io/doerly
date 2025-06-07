using Doerly.Common.Settings;
using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Authorization.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Doerly.Module.Authorization.Domain.Handlers.Users;

public class ChangeUserStateHandler : BaseAuthHandler
{
    public ChangeUserStateHandler(AuthorizationDbContext dbContext, IOptions<AuthSettings> authOptions) : base(
        dbContext, authOptions)
    {
    }

    public async Task<HandlerResult> HandleAsync(int userId, bool isEnabled)
    {
        var userExists = await DbContext.Users.AnyAsync(x => x.Id == userId);
        if (!userExists)
            return HandlerResult.Failure(Resources.Get("UserNotFound"));

        await DbContext.Users
            .Where(x => x.Id == userId)
            .ExecuteUpdateAsync(calls =>
                calls.SetProperty(x => x.IsEnabled, isEnabled));

        return HandlerResult.Success();
    }
}