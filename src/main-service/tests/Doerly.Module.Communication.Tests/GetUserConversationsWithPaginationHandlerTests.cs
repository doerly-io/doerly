using Doerly.DataTransferObjects.Pagination;
using Doerly.Domain.Models;
using Doerly.Module.Communication.DataAccess.Entities;
using Doerly.Module.Communication.Domain.Handlers;
using Doerly.Module.Communication.Enums;
using Doerly.Module.Communication.Tests.Fixtures;
using Doerly.Module.Profile.DataTransferObjects;
using Xunit;

namespace Doerly.Module.Communication.Tests;

public class GetUserConversationsWithPaginationHandlerTests : BaseCommunicationTests
{
    private readonly GetUserConversationsWithPaginationHandler _handler;

    public GetUserConversationsWithPaginationHandlerTests(PostgresTestContainerFixture fixture) : base(fixture)
    {
        _handler = new GetUserConversationsWithPaginationHandler(DbContext, ProfileModuleMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldReturnPaginatedConversations()
    {
        // Arrange
        await SetupUserProfiles(1, 2, 3);

        var conversations = new List<ConversationEntity>
        {
            new()
            {
                InitiatorId = 1,
                RecipientId = 2
            },
            new()
            {
                InitiatorId = 1,
                RecipientId = 3
            }
        };

        await DbContext.Conversations.AddRangeAsync(conversations);
        await DbContext.SaveChangesAsync();

        var pagination = new GetEntitiesWithPaginationRequest
        {
            PageInfo = new PageInfo()
            {
                Number = 1,
                Size = 10
            }
        };

        // Act
        var result = await _handler.HandleAsync(1, pagination);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(2, result.Value.Total);
        Assert.Equal(2, result.Value.Conversations.Count);
        Assert.All(result.Value.Conversations, conversation =>
        {
            Assert.NotNull(conversation.Initiator);
            Assert.NotNull(conversation.Recipient);
            Assert.Equal(1, conversation.Initiator.Id);
        });
    }
} 