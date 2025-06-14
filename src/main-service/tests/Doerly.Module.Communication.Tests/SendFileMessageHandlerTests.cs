using Doerly.Domain.Models;
using Doerly.FileRepository;
using Doerly.Module.Communication.DataTransferObjects.Requests;
using Doerly.Module.Communication.DataAccess.Entities;
using Doerly.Module.Communication.Domain.Handlers;
using Doerly.Module.Communication.Enums;
using Doerly.Module.Communication.Tests.Fixtures;
using Moq;
using Xunit;

namespace Doerly.Module.Communication.Tests;

public class SendFileMessageHandlerTests : BaseCommunicationTests
{
    private readonly SendFileMessageHandler _handler;

    public SendFileMessageHandlerTests(PostgresTestContainerFixture fixture) : base(fixture)
    {
        _handler = new SendFileMessageHandler(DbContext, FileRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldSendFileMessage()
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

        var fileBytes = new byte[] { 37, 80, 68, 70 }; // Example PDF file header bytes
        var fileName = "test.pdf";

        FileRepositoryMock
            .Setup(x => x.UploadFileAsync(
                It.IsAny<string>(),
                It.Is<string>(path => path.StartsWith("communication-files/")),
                It.IsAny<byte[]>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.HandleAsync(conversation.Id, conversation.InitiatorId, fileBytes, fileName);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEqual(0, result.Value);

        var savedMessage = await DbContext.Messages.FindAsync(result.Value);
        Assert.NotNull(savedMessage);
        Assert.StartsWith("communication-files/", savedMessage.MessageContent);
        Assert.Equal(conversation.Id, savedMessage.ConversationId);
        Assert.Equal(conversation.InitiatorId, savedMessage.SenderId);
        Assert.Equal(EMessageType.File, savedMessage.MessageType);
        Assert.Equal(EMessageStatus.Sent, savedMessage.Status);
    }

    [Fact]
    public async Task Handle_NonExistingConversation_ShouldReturnFailure()
    {
        // Arrange
        var fileBytes = new byte[] { 37, 80, 68, 70 }; // Example PDF file header bytes
        var fileName = "test.pdf";

        // Act
        var result = await _handler.HandleAsync(999, 1, fileBytes, fileName);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(0, result.Value);
    }

    [Fact]
    public async Task Handle_UnauthorizedSender_ShouldReturnFailure()
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

        var fileBytes = new byte[] { 1, 2, 3, 4, 5 };
        var fileName = "test.pdf";

        // Act
        var result = await _handler.HandleAsync(conversation.Id, 999, fileBytes, fileName); // Unauthorized sender

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(0, result.Value);
    }

    [Fact]
    public async Task Handle_InvalidFile_ShouldReturnFailure()
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

        var fileBytes = Array.Empty<byte>();
        var fileName = "test.pdf";

        // Act
        var result = await _handler.HandleAsync(conversation.Id, conversation.InitiatorId, fileBytes, fileName);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(0, result.Value);
    }
} 