using Doerly.Domain.Models;
using Doerly.Module.Communication.DataTransferObjects.Requests;
using Doerly.Module.Communication.DataAccess.Entities;
using Doerly.Module.Communication.Domain.Handlers;
using Doerly.Module.Communication.Tests.Fixtures;
using Xunit;

namespace Doerly.Module.Communication.Tests;

public class CreateConversationHandlerTests : BaseCommunicationTests
{
    private readonly CreateConversationHandler _handler;

    public CreateConversationHandlerTests(PostgresTestContainerFixture fixture) : base(fixture)
    {
        _handler = new CreateConversationHandler(DbContext, ProfileModuleMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldCreateConversation()
    {
        // Arrange
        await SetupUserProfiles(1, 2);

        var request = new CreateConversationRequest
        {
            RecipientId = 2
        };

        // Act
        var result = await _handler.HandleAsync(request, 1);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value);

        var savedConversation = await DbContext.Conversations.FindAsync(result.Value);
        Assert.NotNull(savedConversation);
        Assert.Equal(1, savedConversation.InitiatorId);
        Assert.Equal(2, savedConversation.RecipientId);
    }

    [Fact]
    public async Task Handle_SameUserIds_ShouldReturnFailure()
    {
        // Arrange
        await SetupUserProfiles(1);

        var request = new CreateConversationRequest
        {
            RecipientId = 1
        };

        // Act
        var result = await _handler.HandleAsync(request, 1);

        // Assert
        Assert.False(result.IsSuccess);
    }
} 