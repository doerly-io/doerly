using System.Security.Cryptography;
using System.Text;
using Doerly.Common;
using Doerly.Domain.Models;
using Doerly.Module.Authorization.DataAccess;
using Doerly.Module.Authorization.DataAccess.Models;
using Doerly.Module.Authorization.Domain.Dtos;
using Doerly.Module.Authorization.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Doerly.Module.Authorization.Domain.Handlers;

public class RegisterHandler : BaseAuthHandler
{
    public RegisterHandler(AuthorizationDbContext dbContext, IOptions<JwtSettings> jwtOptions) : base(dbContext, jwtOptions)
    {
    }

    public async Task<HandlerResult> HandleAsync(RegisterDto dto)
    {
        var userExist = DbContext.Users.AsNoTracking().Any(x => x.Email == dto.Email);
        if (userExist)
            return HandlerResult.Failure(Resources.Get("UserAlreadyExist"));

        var (passwordHash, passwordSalt) = GetHash(dto.Password);

        var user = new User
        {
            Email = dto.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
        };

        DbContext.Users.Add(user);
        await DbContext.SaveChangesAsync();

        return HandlerResult.Success();
    }

    private (string passwordHash, string passwordSalt) GetHash(string password)
    {
        using var hmac = new HMACSHA512();
        var passwordSalt = Convert.ToBase64String(hmac.Key);
        var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return (Convert.ToBase64String(passwordHash), passwordSalt);
    }
}
