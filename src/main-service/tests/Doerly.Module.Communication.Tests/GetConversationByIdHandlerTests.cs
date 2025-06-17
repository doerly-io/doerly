using Doerly.Domain.Models;
using Doerly.FileRepository;
using Doerly.Module.Communication.DataAccess.Entities;
using Doerly.Module.Communication.Domain.Handlers;
using Doerly.Module.Communication.Tests.Fixtures;
using Doerly.Proxy.Profile;
using Moq;
using Xunit;

namespace Doerly.Module.Communication.Tests;

public class GetConversationByIdHandlerTests : BaseCommunicationTests
{
    private readonly GetConversationByIdHandler _handler;

    public GetConversationByIdHandlerTests(PostgresTestContainerFixture fixture) : base(fixture)
    {

        _handler = new GetConversationByIdHandler(DbContext, ProfileModuleMock.Object, FileRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingConversation_ShouldReturnConversation()
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

        // Act
        var result = await _handler.HandleAsync(conversation.Id);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(conversation.Id, result.Value.Id);
        Assert.Equal(conversation.InitiatorId, result.Value.Initiator.Id);
        Assert.Equal(conversation.RecipientId, result.Value.Recipient.Id);
    }

    [Fact]
    public async Task Handle_NonExistingConversation_ShouldReturnFailure()
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