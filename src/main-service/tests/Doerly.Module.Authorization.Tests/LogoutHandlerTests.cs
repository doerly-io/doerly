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

    public LogoutHandlerTests(PostgresTestContainerFixture fixture) : base(fixture)
    {
        var authSettings = Options.Create(new AuthSettings
        {
            Issuer = "test-issuer",
            Audience = "test-audience",
            SecretKey = Convert.ToBase64String("super_secret_key_1234567890123456"u8.ToArray()), // 32+ bytes
            AccessTokenLifetime = 5,
            RefreshTokenLifetime = 15
        });

        _handler = new LogoutHandler(DbContext, authSettings);
    }
    
    [Fact]
    public async Task HandleAsync_TokenExists_RemovesTokenAndReturnsSuccess()
    {
        // Arrange
        var refreshToken = "test-refresh-token";
        var tokenBytes = Encoding.UTF8.GetBytes(refreshToken);
        using var hmac = new System.Security.Cryptography.HMACSHA512();
        var hashedToken = Convert.ToBase64String(hmac.ComputeHash(tokenBytes));
        var tokenEntity = new TokenEntity
        {
            Value = hashedToken,
            TokenKind = ETokenKind.RefreshToken,
            Guid = Guid.CreateVersion7()
        };
        
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
