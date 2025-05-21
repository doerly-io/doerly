using System.Security.Cryptography;
using System.Text;
using Doerly.Common.Settings;
using Doerly.Localization;
using Doerly.Module.Authorization.DataAccess.Entities;
using Doerly.Module.Authorization.Domain.Handlers;
using Doerly.Module.Authorization.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Doerly.Module.Authorization.Tests;

public class LogoutHandlerTests : BaseAuthTests
{
    private readonly LogoutHandler _handler;
    private readonly IOptions<AuthSettings> _authSettings;

    public LogoutHandlerTests(PostgresTestContainerFixture fixture) : base(fixture)
    {
        _authSettings = Options.Create(new AuthSettings
        {
            Issuer = "test-issuer",
            Audience = "test-audience",
            SecretKey = Convert.ToBase64String("super_secret_key_1234567890123456"u8.ToArray()), // 32+ bytes
            AccessTokenLifetime = 5,
            RefreshTokenLifetime = 15
        });

        _handler = new LogoutHandler(DbContext, _authSettings);
    }
    
    [Fact]
    public async Task HandleAsync_TokenExists_RemovesTokenAndReturnsSuccess()
    {
        // Arrange
        var refreshToken = "test-refresh-token";
        var tokenBytes = Encoding.UTF8.GetBytes(refreshToken);
        var keyBytes = Encoding.UTF8.GetBytes(_authSettings.Value.SecretKey);
        var hashedToken = Convert.ToBase64String(HMACSHA256.HashData(keyBytes, tokenBytes));
        
        var user = new UserEntity
        {
            Id = 1,
            Email = "test@",
            IsEmailVerified = true,
            PasswordSalt = string.Empty,
            PasswordHash = string.Empty,
        };
        
        var tokenEntity = new TokenEntity
        {
            Value = hashedToken,
            TokenKind = ETokenKind.RefreshToken,
            Guid = Guid.CreateVersion7(),
            UserId = user.Id,
            User = user
        };
        
        DbContext.Users.Add(user);
        DbContext.Tokens.Add(tokenEntity);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _handler.HandleAsync(refreshToken);

        // Assert
        Assert.True(result.IsSuccess);
        var tokenInDb = await DbContext.Tokens.FirstOrDefaultAsync(x => x.Value == hashedToken);
        Assert.Null(tokenInDb);
    }
    
    
    [Fact]
    public async Task HandleAsync_TokenDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var refreshToken = "non-existent-refresh-token";

        // Act
        var result = await _handler.HandleAsync(refreshToken);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(Resources.Get("UserNotFound"), result.ErrorMessage);
    }
    
    
}
