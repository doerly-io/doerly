using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Communication.Contracts.Dtos.Requests;
using Doerly.Module.Communication.Contracts.Dtos.Responses;
using Doerly.Module.Communication.DataAccess;
using Doerly.Module.Communication.DataAccess.Entities;
using Doerly.Module.Communication.Enums;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Communication.Domain.Handlers;

public class SendMessageHandler(CommunicationDbContext dbContext) : BaseCommunicationHandler(dbContext)
{
    private readonly CommunicationDbContext _dbContext = dbContext;

    public async Task<HandlerResult<MessageResponseDto>> HandleAsync(int userId, SendMessageRequest dto)
    {
        var conversation = await _dbContext.Conversations
            .FirstOrDefaultAsync(c => c.Id == dto.ConversationId);
        
        if (conversation == null)
        {
            return HandlerResult.Failure<MessageResponseDto>(Resources.Get("Communication.ConversationNotFound"));
        }
        
        if (conversation.InitiatorId != userId && conversation.RecipientId != userId)
        {
            return HandlerResult.Failure<MessageResponseDto>(Resources.Get("Communication.UnauthorizedSender"));
        }
    
        var message = new MessageEntity
        {
            ConversationId = conversation.Id,
            SenderId = userId,
            MessageContent = dto.MessageContent,
            SentAt = DateTime.UtcNow,
            Status = MessageStatus.Sent,
            MessageType = EMessageType.Text
        };
    
        _dbContext.Messages.Add(message);
        await _dbContext.SaveChangesAsync();
        
        var response = new MessageResponseDto
        {
            Id = message.Id,
            ConversationId = message.ConversationId,
            SenderId = message.SenderId,
            MessageContent = message.MessageContent,
            MessageType = message.MessageType,
            SentAt = message.SentAt,
            Status = message.Status
        };
        
        return HandlerResult.Success(response);
    }
}