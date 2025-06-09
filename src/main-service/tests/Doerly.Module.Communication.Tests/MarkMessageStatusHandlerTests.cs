using Doerly.Domain.Models;
using Doerly.Module.Communication.DataAccess.Entities;
using Doerly.Module.Communication.Domain.Handlers;
using Doerly.Module.Communication.Enums;
using Doerly.Module.Communication.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Doerly.Module.Communication.Tests;

public class MarkMessageStatusHandlerTests : BaseCommunicationTests
{
    private readonly MarkMessageStatusHandler _handler;

    public MarkMessageStatusHandlerTests(PostgresTestContainerFixture fixture) : base(fixture)
    {
        _handler = new MarkMessageStatusHandler(DbContext);
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldMarkMessageAsRead()
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
            MessageContent = "Test message",
            SentAt = DateTime.UtcNow,
            MessageType = EMessageType.Text,
            Status = EMessageStatus.Sent
        };
        await DbContext.Messages.AddAsync(message);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _handler.HandleAsync(message.Id, EMessageStatus.Read);

        // Assert
        Assert.True(result.IsSuccess);

        var updatedMessage = await DbContext.Messages.FindAsync(message.Id);
        Assert.NotNull(updatedMessage);
        Assert.Equal(EMessageStatus.Read, updatedMessage.Status);
    }

    [Fact]
    public async Task Handle_NonExistingMessage_ShouldReturnFailure()
    {
        // Act
        var result = await _handler.HandleAsync(999, EMessageStatus.Read);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_UpdateMultipleMessages_ShouldMarkAllAsRead()
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

        var messages = new List<MessageEntity>
        {
            new()
            {
                ConversationId = conversation.Id,
                SenderId = conversation.InitiatorId,
                MessageContent = "Test message 1",
                SentAt = DateTime.UtcNow,
                MessageType = EMessageType.Text,
                Status = EMessageStatus.Sent
            },
            new()
            {
                ConversationId = conversation.Id,
                SenderId = conversation.InitiatorId,
                MessageContent = "Test message 2",
                SentAt = DateTime.UtcNow,
                MessageType = EMessageType.Text,
                Status = EMessageStatus.Sent
            }
        };
        await DbContext.Messages.AddRangeAsync(messages);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _handler.HandleAsync(messages.Select(m => m.Id).ToArray(), EMessageStatus.Read);

        // Assert
        Assert.True(result.IsSuccess);

        var updatedMessages = await DbContext.Messages
            .Where(m => messages.Select(x => x.Id).Contains(m.Id))
            .ToListAsync();
        Assert.Equal(2, updatedMessages.Count);
        Assert.All(updatedMessages, m => Assert.Equal(EMessageStatus.Read, m.Status));
    }
} 