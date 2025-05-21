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

        
        _handler = new RegistrationHandler(DbContext, authSettings,_messagePublisherMock.Object);
        
    }
}