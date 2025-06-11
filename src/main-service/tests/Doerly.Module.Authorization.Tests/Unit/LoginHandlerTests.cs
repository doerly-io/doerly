using System.Text;
using Doerly.Common.Settings;
using Doerly.Module.Authorization.DataAccess.Entities;
using Doerly.Module.Authorization.DataTransferObjects.Requests;
using Doerly.Module.Authorization.Domain.Handlers;
using Microsoft.Extensions.Options;

namespace Doerly.Module.Authorization.Tests;

public class LoginHandlerTests : BaseAuthTests
{
    private readonly LoginHandler _handler;

    public LoginHandlerTests(PostgresTestContainerFixture fixture) : base(fixture)
    {
        var authSettings = Options.Create(new AuthSettings
        {
            Issuer = "test-issuer",
            Audience = "test-audience",
            SecretKey = Convert.ToBase64String("super_secret_key_1234567890123456"u8.ToArray()), // 32+ bytes
            AccessTokenLifetime = 5,
            RefreshTokenLifetime = 15
        });

        _handler = new LoginHandler(DbContext, authSettings);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnSuccess_WhenCredentialsAreValid()
    {
        // Arrange
        var email = "user@test.com";
        var password = "pass123";

        using var hmac = new System.Security.Cryptography.HMACSHA512();
        var salt = Convert.ToBase64String(hmac.Key);
        var hash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));

        var role = new RoleEntity { Id = 1, Name = "User" };
        var user = new UserEntity
        {
            Id = 1,
            Email = email,
            IsEmailVerified = true,
            PasswordSalt = salt,
            PasswordHash = hash,
            Role = role,
            RoleId = role.Id
        };

        DbContext.Roles.Add(role);
        DbContext.Users.Add(user);
        await DbContext.SaveChangesAsync();

        var request = new LoginRequestDto { Email = email, Password = password };

        // Act
        var result = await _handler.HandleAsync(request);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(email, result.Value.resultDto.UserEmail);
        Assert.False(string.IsNullOrEmpty(result.Value.resultDto.AccessToken));
        Assert.False(string.IsNullOrEmpty(result.Value.refreshToken));
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnFailure_WhenUserNotFound()
    {
        var request = new LoginRequestDto { Email = "no@one.com", Password = "pass" };

        var result = await _handler.HandleAsync(request);

        Assert.False(result.IsSuccess);
        Assert.Contains("user", result.ErrorMessage, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnFailure_WhenPasswordIsWrong()
    {
        var email = "test@wrong.com";
        var correctPassword = "correct";
        var wrongPassword = "wrong";

        using var hmac = new System.Security.Cryptography.HMACSHA512();
        var salt = Convert.ToBase64String(hmac.Key);
        var hash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(correctPassword)));

        var role = new RoleEntity { Id = 2, Name = "Admin" };
        var user = new UserEntity
        {
            Id = 2,
            Email = email,
            IsEmailVerified = true,
            PasswordSalt = salt,
            PasswordHash = hash,
            Role = role,
            RoleId = role.Id
        };

        DbContext.Roles.Add(role);
        DbContext.Users.Add(user);
        await DbContext.SaveChangesAsync();

        var request = new LoginRequestDto { Email = email, Password = wrongPassword };

        var result = await _handler.HandleAsync(request);

        Assert.False(result.IsSuccess);
        Assert.Contains("password", result.ErrorMessage, StringComparison.OrdinalIgnoreCase);
    }
}
