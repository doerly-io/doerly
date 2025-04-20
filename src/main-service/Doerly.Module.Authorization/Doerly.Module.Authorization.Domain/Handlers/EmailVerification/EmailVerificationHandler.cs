using Doerly.Common.Settings;
using Doerly.Domain.Models;
using Doerly.Module.Authorization.DataAccess;
using Doerly.Module.Authorization.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Doerly.Module.Authorization.Domain.Handlers;

public class EmailVerificationHandler : BaseAuthHandler
{
    public EmailVerificationHandler(AuthorizationDbContext dbContext, IOptions<AuthSettings> authOptions) : base(dbContext, authOptions)
    {
    }

    public async Task<HandlerResult> HandleAsync(string token, string email)
    {
        var tokenBytes = Convert.FromBase64String(token);
        var tokenHash = GetResetTokenHash(tokenBytes);

        var tokenEntity = await DbContext.Tokens
            .Include(x => x.User)
            .Where(x => x.TokenKind == ETokenKind.EmailVerification && x.User.Email == email && x.Value == tokenHash)
            .FirstOrDefaultAsync();

        if (tokenEntity == null)
            return HandlerResult.Failure("Invalid token");

        tokenEntity.User.IsEmailVerified = true;
        DbContext.Tokens.Remove(tokenEntity);
        await DbContext.SaveChangesAsync();

        return HandlerResult.Success();
    }
}
