using System.Text.Json;
using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Communication.Contracts.Requests;
using Doerly.Module.Communication.DataAccess;
using Doerly.Module.Communication.DataAccess.Entities;
using Doerly.Module.Communication.Enums;
using Doerly.Module.Notification.Enums;
using Doerly.Module.Notification.Proxy;
using Doerly.Proxy.Profile;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Communication.Domain.Handlers;

public class SendMessageHandler(CommunicationDbContext dbContext, INotificationModuleProxy notificationModuleProxy, IProfileModuleProxy profileModuleProxy) : BaseCommunicationHandler(dbContext)
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
        
        var senderName = $"{sender.Value.FirstName} {sender.Value.LastName}";
        // Notify the conversation participants about the new message
        await notificationModuleProxy.SendNotificationAsync(
            conversation.RecipientId == userId ? conversation.InitiatorId : conversation.RecipientId,
            Resources.Get("Notification.NewMessage", senderName),
            dto.MessageContent,
            NotificationType.Message,
            JsonSerializer.Serialize(new { conversationId = conversation.Id, messageId = message.Id })
        );
        
        return OperationResult.Success(message.Id);
    }
}