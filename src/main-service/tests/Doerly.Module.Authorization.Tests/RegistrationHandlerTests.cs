using Doerly.Common.Settings;
using Doerly.Messaging;
using Doerly.Module.Authorization.Domain.Handlers;
using Microsoft.Extensions.Options;
using Moq;

namespace Doerly.Module.Authorization.Tests;

public class RegistrationHandlerTests : BaseAuthTests
{
    private readonly RegistrationHandler _handler;
    private readonly Mock<IMessagePublisher> _messagePublisherMock;

    public RegistrationHandlerTests(PostgresTestContainerFixture fixture) : base(fixture)
    {
        var authSettings = Options.Create(new AuthSettings
        {
            Issuer = "test-issuer",
            Audience = "test-audience",
            SecretKey = Convert.ToBase64String("super_secret_key_1234567890123456"u8.ToArray()), // 32+ bytes
            AccessTokenLifetime = 5,
            RefreshTokenLifetime = 15
        });

        _messagePublisherMock = new Mock<IMessagePublisher>();

        _handler = new RegistrationHandler(DbContext, authSettings, _messagePublisherMock.Object);
    }

    [Fact]
    public async Task HandleAsync_ShouldRegisterUser_AndPublishMessage()
    {
        // Arrange
        var email = "newuser@test.com";
        var password = "TestPassword123!";
        var firstName = "Test";
        var lastName = "User";
        var registerRequest = new DataTransferObjects.Requests.RegisterRequestDto
        {
            Email = email,
            Password = password,
            FirstName = firstName,
            LastName = lastName
        };

        // Act
        var result = await _handler.HandleAsync(registerRequest);

        // Assert: Registration succeeded
        Assert.True(result.IsSuccess);

        // Assert: User exists in DB
        var user = DbContext.Users.FirstOrDefault(u => u.Email == email);
        Assert.NotNull(user);
        Assert.Equal(email, user.Email);

        // Assert: Message published
        _messagePublisherMock.Verify(m => m.Publish(
            It.Is<DataTransferObjects.Messages.UserRegisteredMessage>(msg =>
                msg.Email == email && msg.FirstName == firstName && msg.LastName == lastName),
            CancellationToken.None), Times.Once);
    }


[Fact]
    public async Task HandleAsync_ShouldReturnFailure_WhenUserAlreadyExists()
    {
        // Arrange
        var email = "duplicate@test.com";
        var password = "TestPassword123!";
        var firstName = "Test";
        var lastName = "User";
        var registerRequest = new DataTransferObjects.Requests.RegisterRequestDto
        {
            Email = email,
            Password = password,
            FirstName = firstName,
            LastName = lastName
        };

        // Register the user the first time
        var firstResult = await _handler.HandleAsync(registerRequest);
        Assert.True(firstResult.IsSuccess);

        // Act: Try to register again with the same email
        var secondResult = await _handler.HandleAsync(registerRequest);

        // Assert: Should fail
        Assert.False(secondResult.IsSuccess);
        Assert.Contains("exist", secondResult.ErrorMessage, StringComparison.OrdinalIgnoreCase);

        // Assert: No message published for duplicate registration
        _messagePublisherMock.Verify(m => m.Publish(
            It.IsAny<DataTransferObjects.Messages.UserRegisteredMessage>(),
            CancellationToken.None), Times.Once); // Only the first registration should publish
    }


}
