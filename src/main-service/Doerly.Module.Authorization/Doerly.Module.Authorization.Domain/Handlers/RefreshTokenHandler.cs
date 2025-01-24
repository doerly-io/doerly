using Doerly.Common;
using Doerly.Domain.Models;
using Doerly.Module.Authorization.DataAccess;
using Doerly.Module.Authorization.Domain.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Doerly.Module.Authorization.Domain.Handlers;

public class RefreshTokenHandler : BaseAuthHandler
{
    public RefreshTokenHandler(AuthorizationDbContext dbContext, IOptions<JwtSettings> options) : base(dbContext, options)
    {
    }

    public async Task<HandlerResult<(LoginResultDto resultDto, string refreshToken)>> HandleAsync(string refreshToken, string accessToken)
    {
        var token = await DbContext.RefreshTokens
            .AsNoTracking()
            .Include(x => x.User)
            .Where(x => x.Guid == Guid.Parse(refreshToken))
            .Select(x => new { x.Guid, x.User.Email, x.UserId, Role = x.User.Role.Name })
            .FirstOrDefaultAsync();

        if (token == null)
            return HandlerResult.Failure<(LoginResultDto resultDto, string refreshToken)>("Unauthorized");

        var accessTokenNew = CreateAccessToken(token.UserId, token.Email, token.Role);
        var loginResultDto = new LoginResultDto
        {
            UserEmail = token.Email,
            AccessToken = accessTokenNew
        };

        var refreshTokenNew = Guid.NewGuid();
        await CreateRefreshTokenAsync(refreshTokenNew, token.UserId);

        return HandlerResult.Success((loginResultDto, refreshTokenNew.ToString()));
    }
}
