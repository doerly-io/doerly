using Doerly.Common;
using Doerly.Domain.Models;
using Doerly.Module.Authorization.DataAccess;
using Doerly.Module.Authorization.DataAccess.Models;
using Doerly.Module.Authorization.Domain.Dtos;
using Doerly.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Doerly.Module.Authorization.Domain.Handlers;

public class RegisterHandler : BaseAuthHandler
{
    public RegisterHandler(AuthorizationDbContext dbContext, IOptions<AuthSettings> jwtOptions) : base(dbContext, jwtOptions)
    {
    }

    public async Task<HandlerResult> HandleAsync(RegisterDto dto)
    {
        var userExist = DbContext.Users.AsNoTracking().Any(x => x.Email == dto.Email);
        if (userExist)
            return HandlerResult.Failure(Resources.Get("UserAlreadyExist"));

        var (passwordHash, passwordSalt) = GetPasswordHash(dto.Password);

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
    
}
