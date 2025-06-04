using Doerly.Common.Settings;
using Doerly.Domain.Factories;
using Doerly.Domain.Models;
using Doerly.Module.Authorization.DataAccess;
using Doerly.Localization;
using Doerly.Module.Authorization.Contracts.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Doerly.Module.Authorization.Domain.Handlers;

public class ResetPasswordHandler : BaseAuthHandler
{
    private readonly IHandlerFactory _handlerFactory;

    public ResetPasswordHandler(
        AuthorizationDbContext dbContext,
        IOptions<AuthSettings> authOptions,
        IHandlerFactory handlerFactory
    ) : base(dbContext, authOptions)
    {
        _handlerFactory = handlerFactory;
    }

    public async Task<HandlerResult> HandleAsync(ResetPasswordRequestDto requestDto)
    {
        var tokenBytes = Convert.FromBase64String(requestDto.Token);
        var hashedToken = GetResetTokenHash(tokenBytes);

        var resetToken = await DbContext.Tokens.AsNoTracking()
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.User.Email == requestDto.Email && x.Value == hashedToken);

        if (resetToken == null)
            return HandlerResult.Failure(Resources.Get("ValidTokenNotFound"));

        if (resetToken.DateCreated.AddMinutes(AuthOptions.Value.PasswordResetTokenLifetime) < DateTime.UtcNow)
        {
            await DbContext.Tokens.Where(x => x.Guid == resetToken.Guid).ExecuteDeleteAsync();
            return HandlerResult.Failure(Resources.Get("ValidTokenNotFound"));
        }

        var user = await DbContext.Users.FirstOrDefaultAsync(x => x.Id == resetToken.UserId);
        if (user == null)
            return HandlerResult.Failure(Resources.Get("UserNotFound"));

        var updatePasswordResult = await _handlerFactory.Get<UpdateUserPasswordHandler>().HandleAsync(user, requestDto.Password);
        if (updatePasswordResult.IsSuccess)
            await DbContext.Tokens.Where(x => x.Guid == resetToken.Guid).ExecuteDeleteAsync();

        return updatePasswordResult;
    }
}
