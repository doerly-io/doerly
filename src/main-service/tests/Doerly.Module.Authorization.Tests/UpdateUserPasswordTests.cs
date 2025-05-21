using System.Security.Cryptography;
using System.Text;
using Doerly.Common.Settings;
using Doerly.Module.Authorization.DataAccess.Entities;
using Doerly.Module.Authorization.Domain.Handlers;
using Microsoft.Extensions.Options;

namespace Doerly.Module.Authorization.Tests;

public class UpdateUserPasswordTests : BaseAuthTests
{
    private readonly UpdateUserPasswordHandler _handler;

    public UpdateUserPasswordTests(PostgresTestContainerFixture fixture) : base(fixture)
    {
        var authSettings = Options.Create(new AuthSettings
        {
            Issuer = "test-issuer",
            Audience = "test-audience",
            SecretKey = Convert.ToBase64String("super_secret_key_1234567890123456"u8.ToArray()), // 32+ bytes
            AccessTokenLifetime = 5,
            RefreshTokenLifetime = 15
        });

        _handler = new UpdateUserPasswordHandler(DbContext, authSettings);
    }

    [Fact]
    public async Task HandleAsync_ValidUserAndPassword_UpdatesPasswordAndReturnsSuccess()
    {
        // Arrange
        var user = new UserEntity
        {
            Id = 1,
            Email = "user@test.com",
            PasswordHash = string.Empty,
            PasswordSalt = string.Empty
        };
        DbContext.Users.Add(user);
        await DbContext.SaveChangesAsync();

        var password = "testpassword";

        // Act
        var result = await _handler.HandleAsync(user, password);

        // Assert
        Assert.True(result.IsSuccess);
        var updatedUser = await DbContext.Users.FindAsync(1);
        Assert.NotNull(updatedUser);
    }
}
