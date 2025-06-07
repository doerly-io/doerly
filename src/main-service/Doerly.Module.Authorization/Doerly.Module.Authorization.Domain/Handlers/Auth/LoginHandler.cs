using System.Security.Cryptography;
using System.Text;
using Doerly.Common.Settings;
using Doerly.Domain.Models;
using Doerly.Module.Authorization.DataAccess;
using Doerly.Localization;
using Doerly.Module.Authorization.Contracts.Requests;
using Doerly.Module.Authorization.Contracts.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Doerly.Module.Authorization.Domain.Handlers;

public class LoginHandler : BaseAuthHandler
{
    public LoginHandler(AuthorizationDbContext dbContext, IOptions<AuthSettings> options) : base(dbContext, options)
    {
    }

    public async Task<HandlerResult<(LoginResponseDto resultDto, string refreshToken)>> HandleAsync(LoginRequestDto requestDto)
    {
        var user = await DbContext.Users
            .AsNoTracking()
            .Include(user => user.Role)
            .Where(x => x.Email == requestDto.Email && x.IsEmailVerified && x.IsEnabled)
            .Select(x => new
            {
                x.Id,
                x.Email,
                x.PasswordHash,
                x.PasswordSalt,
                RoleName = x.Role.Name
            })
            .FirstOrDefaultAsync();

        if (user == null)
            return HandlerResult.Failure<(LoginResponseDto, string)>(Resources.Get("UserNotFound"));

        if (!VerifyPasswordHash(requestDto.Password, user.PasswordHash, user.PasswordSalt))
            return HandlerResult.Failure<(LoginResponseDto, string)>(Resources.Get("InvalidPassword"));

        var accessToken = CreateAccessToken(user.Id, user.Email, user.RoleName);
        var refreshTokenValue = GetResetToken();
        await CreateRefreshTokenAsync(refreshTokenValue.hashedToken, user.Id);

        var loginResultDto = new LoginResponseDto
        {
            AccessToken = accessToken,
            UserEmail = user.Email
        };

        return HandlerResult.Success((loginResultDto, refreshTokenValue.originalToken));
    }

    private bool VerifyPasswordHash(string password, string passwordHash, string passwordSalt)
    {
        using var hmac = new HMACSHA512(Convert.FromBase64String(passwordSalt));
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(Convert.FromBase64String(passwordHash));
    }
}
