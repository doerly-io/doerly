using Doerly.Domain.Models;
using Doerly.Module.Communication.DataAccess.Entities;
using Doerly.Module.Communication.Domain.Handlers;
using Doerly.Module.Communication.Tests.Fixtures;
using Xunit;

namespace Doerly.Module.Communication.Tests;

public class CheckConversationExistsHandlerTests : BaseCommunicationTests
{
    private readonly CheckConversationExistsHandler _handler;

    public CheckConversationExistsHandlerTests(PostgresTestContainerFixture fixture) : base(fixture)
    {
        _handler = new CheckConversationExistsHandler(DbContext);
    }

    [Fact]
    public async Task Handle_ExistingConversation_ShouldReturnConversationId()
    {
        // Arrange
        var conversation = new ConversationEntity
        {
            InitiatorId = 1,
            RecipientId = 2
        };
        await DbContext.Conversations.AddAsync(conversation);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _handler.HandleAsync(conversation.InitiatorId, conversation.RecipientId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(conversation.Id, result.Value);
    }

    [Fact]
    public async Task Handle_NonExistingConversation_ShouldReturnNull()
    {
        // Arrange
        await SetupUserProfiles(1, 2);

        // Act
        var result = await _handler.HandleAsync(1, 2);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Null(result.Value);
    }

    [Fact]
    public async Task Handle_SameUserIds_ShouldReturnNull()
    {
        // Arrange
        await SetupUserProfiles(1);

        // Act
        var result = await _handler.HandleAsync(1, 1);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Null(result.Value);
    }
} 