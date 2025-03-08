using System.Security.Cryptography;
using System.Text;
using Doerly.Common;
using Doerly.Domain.Models;
using Doerly.Module.Authorization.DataAccess;
using Doerly.Module.Authorization.Domain.Dtos;
using Doerly.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Doerly.Module.Authorization.Domain.Handlers;

public class LoginHandler : BaseAuthHandler
{
    public LoginHandler(AuthorizationDbContext dbContext, IOptions<AuthSettings> options) : base(dbContext, options)
    {
    }

    public async Task<HandlerResult<(LoginResultDto resultDto, string refreshToken)>> HandleAsync(LoginDto dto)
    {
        var user = await DbContext.Users
            .AsNoTracking()
            .Include(user => user.Role)
            .Where(x => x.Email == dto.Email)
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
            return HandlerResult.Failure<(LoginResultDto, string)>(Resources.Get("UserNotFound"));

        if (!VerifyPasswordHash(dto.Password, user.PasswordHash, user.PasswordSalt))
            return HandlerResult.Failure<(LoginResultDto, string)>(Resources.Get("InvalidPassword"));

        var accessToken = CreateAccessToken(user.Id, user.Email, user.RoleName);
        var refreshTokenValue = GetResetToken();
        await CreateRefreshTokenAsync(refreshTokenValue.hashedToken, user.Id);

        var loginResultDto = new LoginResultDto
        {
            AccessToken = accessToken,
            UserEmail = user.Email
        };

        return HandlerResult.Success((loginResultDto, refreshTokenValue.hashedToken));
    }

    private bool VerifyPasswordHash(string password, string passwordHash, string passwordSalt)
    {
        using var hmac = new HMACSHA512(Convert.FromBase64String(passwordSalt));
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(Convert.FromBase64String(passwordHash));
    }
}
