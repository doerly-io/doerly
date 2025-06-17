using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Doerly.Domain.Handlers;
using Doerly.Common.Settings;
using Doerly.Module.Authorization.DataAccess;
using Doerly.Module.Authorization.DataAccess.Entities;
using Doerly.Module.Authorization.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Doerly.Module.Authorization.Domain.Handlers;

public class BaseAuthHandler : BaseHandler<AuthorizationDbContext>
{
    private const int TokenLengthByteCount = 32;
    protected readonly IOptions<AuthSettings> AuthOptions;

    public BaseAuthHandler(AuthorizationDbContext dbContext, IOptions<AuthSettings> authOptions) : base(dbContext)
    {
        AuthOptions = authOptions;
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
            Audience = AuthOptions.Value.Audience,
            Issuer = AuthOptions.Value.Issuer,
            Claims = claims,
            IssuedAt = DateTime.UtcNow,
            NotBefore = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddMinutes(AuthOptions.Value.AccessTokenLifetime),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthOptions.Value.SecretKey)),
                SecurityAlgorithms.HmacSha256Signature),
        };

        var handler = new JsonWebTokenHandler();
        var token = handler.CreateToken(tokenDescriptor);
        return token;
    }

    protected async Task CreateRefreshTokenAsync(string tokenValue, int userId)
    {
        await DbContext.Tokens.Where(x => x.UserId == userId && x.TokenKind == ETokenKind.RefreshToken).ExecuteDeleteAsync();

        var refreshToken = new TokenEntity
        {
            UserId = userId,
            Guid = Guid.CreateVersion7(),
            Value = tokenValue,
            TokenKind = ETokenKind.RefreshToken,
            DateCreated = DateTime.UtcNow,
        };

        DbContext.Tokens.Add(refreshToken);
        await DbContext.SaveChangesAsync();
    }

    protected (string passwordHash, string passwordSalt) GetPasswordHash(string password)
    {
        using var hmac = new HMACSHA512();
        var passwordSalt = Convert.ToBase64String(hmac.Key);
        var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return (Convert.ToBase64String(passwordHash), passwordSalt);
    }

    protected int? GetUserIdFromJwtToken(string accessToken)
    {
        var handler = new JsonWebTokenHandler();
        var token = (JsonWebToken)handler.ReadToken(accessToken);
        var userIdClaim = token.TryGetValue<int>(ClaimTypes.NameIdentifier, out var userId);
        return userIdClaim ? userId : null;
    }
    
    protected string GetResetTokenHash(byte[] tokenBytes)
    {
        var secretKey = Encoding.UTF8.GetBytes(AuthOptions.Value.SecretKey);
        var hashedToken = HMACSHA256.HashData( secretKey, tokenBytes);
        return Convert.ToBase64String(hashedToken);
    }

    protected (string hashedToken, string originalToken) GetResetToken()
    {
        var tokenBytes = RandomNumberGenerator.GetBytes(TokenLengthByteCount);
        var originalToken = Convert.ToBase64String(tokenBytes);
        var token = GetResetTokenHash(tokenBytes);
        return (token, originalToken);
    }
}
