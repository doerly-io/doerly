using Doerly.Common.Settings;
using Doerly.Domain.Models;
using Doerly.Module.Authorization.Contracts.Responses;
using Doerly.Module.Authorization.DataAccess;
using Doerly.Module.Authorization.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Doerly.Module.Authorization.Domain.Handlers;

public class RefreshTokenHandler : BaseAuthHandler
{
    public RefreshTokenHandler(AuthorizationDbContext dbContext, IOptions<AuthSettings> options) : base(dbContext,
        options)
    {
    }

    public async Task<HandlerResult<(LoginResponseDto resultDto, string refreshToken)>> HandleAsync(string refreshToken,
        string accessToken)
    {
        var userId = GetUserIdFromJwtToken(accessToken);
        var refreshTokenBytes = Convert.FromBase64String(refreshToken);
        var refreshTokenHash = GetResetTokenHash(refreshTokenBytes);

        var token = await DbContext.Tokens
            .AsNoTracking()
            .Include(x => x.User)
            .Where(x => x.TokenKind == ETokenKind.RefreshToken
                        && x.Value == refreshTokenHash
                        && (userId != null && x.UserId == userId)
                        && x.User.IsEnabled)
            .Select(x => new { x.Guid, x.DateCreated, x.User.Email, x.UserId, Role = x.User.Role.Name })
            .FirstOrDefaultAsync();

        if (token == null || token.DateCreated.AddMinutes(AuthOptions.Value.RefreshTokenLifetime) < DateTime.UtcNow)
            return HandlerResult.Failure<(LoginResponseDto resultDto, string refreshToken)>("Unauthorized");

        var accessTokenNew = CreateAccessToken(token.UserId, token.Email, token.Role);
        var loginResultDto = new LoginResponseDto
        {
            UserEmail = token.Email,
            AccessToken = accessTokenNew
        };

        var refreshTokenNew = GetResetToken();
        await CreateRefreshTokenAsync(refreshTokenNew.hashedToken, token.UserId);

        return HandlerResult.Success((loginResultDto, refreshTokenNew.originalToken));
    }
}