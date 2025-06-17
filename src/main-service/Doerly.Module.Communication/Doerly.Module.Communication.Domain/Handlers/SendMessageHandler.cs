using System.Text.Json;
using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Messaging;
using Doerly.Module.Communication.DataTransferObjects.Messages;
using Doerly.Module.Communication.DataTransferObjects.Requests;
using Doerly.Module.Communication.DataAccess;
using Doerly.Module.Communication.DataAccess.Entities;
using Doerly.Module.Communication.Enums;
using Doerly.Proxy.Profile;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Communication.Domain.Handlers;

public class SendMessageHandler(CommunicationDbContext dbContext, 
    IProfileModuleProxy profileModuleProxy,
    IMessagePublisher messagePublisher) : BaseCommunicationHandler(dbContext)
{
    private readonly CommunicationDbContext _dbContext = dbContext;

    public async Task<OperationResult<int>> HandleAsync(int userId, SendMessageRequest dto)
    {
        var conversation = await _dbContext.Conversations
            .FirstOrDefaultAsync(c => c.Id == dto.ConversationId);
        
        if (conversation == null)
        {
            return OperationResult.Failure<int>(Resources.Get("Communication.ConversationNotFound"));
        }
        
        if (conversation.InitiatorId != userId && conversation.RecipientId != userId)
        {
            return OperationResult.Failure<int>(Resources.Get("Communication.UnauthorizedSender"));
        }
        
        if (string.IsNullOrWhiteSpace(dto.MessageContent))
        {
            return OperationResult.Failure<int>(Resources.Get("Communication.MessageContentCannotBeEmpty"));
        }
    
        var message = new MessageEntity
        {
            ConversationId = conversation.Id,
            SenderId = userId,
            MessageContent = dto.MessageContent,
            SentAt = DateTime.UtcNow,
            Status = EMessageStatus.Sent,
            MessageType = EMessageType.Text
        };
    
        _dbContext.Messages.Add(message);
        await _dbContext.SaveChangesAsync();
        
        // Get sender's name for the notification
        var sender = await profileModuleProxy.GetProfileAsync(userId);
        if (!sender.IsSuccess)
        {
            return OperationResult.Failure<int>(Resources.Get("Communication.UnauthorizedSender"));
        }
        
        // Notify the conversation participants about the new message
        var notificationMessage = new NewMessageNotificationMessage(
            conversation.RecipientId == userId ? conversation.InitiatorId : conversation.RecipientId,
            JsonSerializer.Serialize(new { 
                conversationId = conversation.Id, 
                messageId = message.Id,
                senderName = $"{sender.Value.FirstName} {sender.Value.LastName}",
            }),
            message.SentAt
        );
        await messagePublisher.Publish(notificationMessage);
        
        return OperationResult.Success(message.Id);
    }
}