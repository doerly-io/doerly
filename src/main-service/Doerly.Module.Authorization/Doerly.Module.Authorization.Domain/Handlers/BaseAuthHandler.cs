using System.Security.Claims;
using System.Text;
using Doerly.Domain.Handlers;
using Doerly.Common;
using Doerly.Module.Authorization.DataAccess;
using Doerly.Module.Authorization.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Doerly.Module.Authorization.Domain.Handlers;

public class BaseAuthHandler : BaseHandler<AuthorizationDbContext>
{
    protected readonly IOptions<JwtSettings> JwtOptions;

    public BaseAuthHandler(AuthorizationDbContext dbContext, IOptions<JwtSettings> jwtOptions) : base(dbContext)
    {
        JwtOptions = jwtOptions;
    }

    protected string CreateAccessToken(int userId, string email, string userRole)
    {
        var claims = new Dictionary<string, object>
        {
            [ClaimTypes.NameIdentifier] = userId,
            [ClaimTypes.Email] = email,
            [ClaimTypes.Role] = userRole
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = JwtOptions.Value.Audience,
            Issuer = JwtOptions.Value.Issuer,
            Claims = claims,
            IssuedAt = DateTime.UtcNow,
            NotBefore = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddSeconds(JwtOptions.Value.AccessTokenExpiration),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.Value.SecretKey)),
                SecurityAlgorithms.HmacSha256Signature),
        };

        var handler = new JsonWebTokenHandler();
        var token = handler.CreateToken(tokenDescriptor);
        return token;
    }
    
    protected async Task CreateRefreshTokenAsync(Guid tokenGuid, int userId)
    {
        await DbContext.RefreshTokens.Where(x => x.UserId == userId).ExecuteDeleteAsync();

        var refreshToken = new RefreshToken
        {
            UserId = userId,
            Guid = tokenGuid
        };

        DbContext.RefreshTokens.Add(refreshToken);
        await DbContext.SaveChangesAsync();
    }
}
