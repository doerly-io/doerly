 using Doerly.Domain.Models;
 using Doerly.Messaging;
 using Doerly.Module.Communication.DataTransferObjects.Requests;
 using Doerly.Module.Communication.DataAccess.Entities;
 using Doerly.Module.Communication.Domain.Handlers;
 using Doerly.Module.Communication.Enums;
 using Doerly.Module.Communication.Tests.Fixtures;
 using Doerly.Proxy.Profile;
 using Moq;
 using Xunit;

 namespace Doerly.Module.Communication.Tests;

 public class SendMessageHandlerTests : BaseCommunicationTests
 {
     private readonly SendMessageHandler _handler;

     public SendMessageHandlerTests(PostgresTestContainerFixture fixture) : base(fixture)
     {
         var messagePublisherMock = new Mock<IMessagePublisher>();
         _handler = new SendMessageHandler(DbContext, ProfileModuleMock.Object, messagePublisherMock.Object);
     }

     [Fact]
     public async Task Handle_ValidRequest_ShouldSendMessage()
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

         var request = new SendMessageRequest
         {
             ConversationId = conversation.Id,
             MessageContent = "Test message content"
         };

         // Act
         var result = await _handler.HandleAsync(conversation.InitiatorId, request);

         // Assert
         Assert.True(result.IsSuccess);
         Assert.NotEqual(0, result.Value);

         var savedMessage = await DbContext.Messages.FindAsync(result.Value);
         Assert.NotNull(savedMessage);
         Assert.Equal(request.MessageContent, savedMessage.MessageContent);
         Assert.Equal(request.ConversationId, savedMessage.ConversationId);
         Assert.Equal(conversation.InitiatorId, savedMessage.SenderId);
         Assert.Equal(EMessageType.Text, savedMessage.MessageType);
         Assert.Equal(EMessageStatus.Sent, savedMessage.Status);
     }

     [Fact]
     public async Task Handle_NonExistingConversation_ShouldReturnFailure()
     {
         // Arrange
         var request = new SendMessageRequest
         {
             ConversationId = 999,
             MessageContent = "Test message content"
         };

         // Act
         var result = await _handler.HandleAsync(1, request);

         // Assert
         Assert.False(result.IsSuccess);
         Assert.Equal(0, result.Value);
     }

     [Fact]
     public async Task Handle_UnauthorizedSender_ShouldReturnFailure()
     {
         // Arrange
         var conversation = new ConversationEntity
         {
             InitiatorId = 1,
             RecipientId = 2
         };
         await DbContext.Conversations.AddAsync(conversation);
         await DbContext.SaveChangesAsync();

         var request = new SendMessageRequest
         {
             ConversationId = conversation.Id,
             MessageContent = "Test message content"
         };

         // Act
         var result = await _handler.HandleAsync(999, request); // Unauthorized sender

         // Assert
         Assert.False(result.IsSuccess);
         Assert.Equal(0, result.Value);
     }

     [Fact]
     public async Task Handle_EmptyMessage_ShouldReturnFailure()
     {
         // Arrange
         var conversation = new ConversationEntity
         {
             InitiatorId = 1,
             RecipientId = 2
         };
         await DbContext.Conversations.AddAsync(conversation);
         await DbContext.SaveChangesAsync();

         var request = new SendMessageRequest
         {
             ConversationId = conversation.Id,
             MessageContent = string.Empty
         };

         // Act
         var result = await _handler.HandleAsync(conversation.InitiatorId, request);

         // Assert
         Assert.False(result.IsSuccess);
         Assert.Equal(0, result.Value);
     }
 } 