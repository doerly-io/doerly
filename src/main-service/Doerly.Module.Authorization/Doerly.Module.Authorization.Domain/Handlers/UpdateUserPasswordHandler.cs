using Doerly.Common;
using Doerly.Domain.Models;
using Doerly.Module.Authorization.DataAccess;
using Doerly.Module.Authorization.DataAccess.Models;
using Microsoft.Extensions.Options;

namespace Doerly.Module.Authorization.Domain.Handlers;

public class UpdateUserPasswordHandler : BaseAuthHandler
{
    public UpdateUserPasswordHandler(AuthorizationDbContext dbContext, IOptions<AuthSettings> authOptions) : base(dbContext, authOptions)
    {
    }

    public async Task<HandlerResult> HandleAsync(User user, string password)
    {
        var (passwordHash, passwordSalt) = GetPasswordHash(password);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
        await DbContext.SaveChangesAsync();

        return HandlerResult.Success();
    }
}
