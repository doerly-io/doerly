using Doerly.Module.Communication.DataAccess.Entities;
using Doerly.Module.Communication.Domain.Handlers;
using Doerly.Module.Communication.Enums;
using Doerly.Module.Communication.Tests.Fixtures;
using Doerly.Proxy.Profile;
using Moq;
using Xunit;

namespace Doerly.Module.Communication.Tests;

public class GetMessageByIdHandlerTests : BaseCommunicationTests
{
    private readonly GetMessageByIdHandler _handler;

    public GetMessageByIdHandlerTests(PostgresTestContainerFixture fixture) : base(fixture)
    {
        _handler = new GetMessageByIdHandler(DbContext, ProfileModuleMock.Object, FileRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingMessage_ShouldReturnMessage()
    {
        // Arrange
        await SetupUserProfiles(1, 2);
        var conversation = new ConversationEntity
        {
            InitiatorId = 1,
            RecipientId = 2
        };
        await DbContext.Conversations.AddAsync(conversation);
        await DbContext.SaveChangesAsync();

        var message = new MessageEntity
        {
            ConversationId = conversation.Id,
            SenderId = conversation.InitiatorId,
            MessageContent = "Test message content",
            SentAt = DateTime.UtcNow,
            MessageType = EMessageType.Text,
            Status = EMessageStatus.Sent
        };
        await DbContext.Messages.AddAsync(message);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _handler.HandleAsync(message.Id);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(message.Id, result.Value.Id);
        Assert.Equal(message.MessageContent, result.Value.MessageContent);
        Assert.Equal(message.ConversationId, result.Value.ConversationId);
        Assert.Equal(message.SenderId, result.Value.SenderId);
        Assert.Equal(message.MessageType, result.Value.MessageType);
        Assert.Equal(message.Status, result.Value.Status);
    }

    [Fact]
    public async Task Handle_NonExistingMessage_ShouldReturnFailure()
    {
        // Arrange
        var nonExistingId = 999;

        // Act
        var result = await _handler.HandleAsync(nonExistingId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Null(result.Value);
    }
} 