using Doerly.Domain.Models;
using Doerly.Module.Communication.Contracts.Dtos.Requests;
using Doerly.Module.Communication.DataAccess;
using Doerly.Module.Communication.DataAccess.Entities;
using Doerly.Module.Communication.Enums;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Communication.Domain.Handlers;

public class SendMessageHandler(CommunicationDbContext dbContext) : BaseCommunicationHandler(dbContext)
{
    private readonly CommunicationDbContext _dbContext = dbContext;

    public async Task<HandlerResult> HandleAsync(SendMessageDto dto)
    {
        var conversation = await _dbContext.Conversations.FirstOrDefaultAsync(c => c.Id == dto.ConversationId);
    
        if (conversation == null)
        {
            //TODO: Add profiles validation sending requests to ProfileModule
            conversation = new ConversationEntity
            {
                InitiatorId = dto.InitiatorId,
                RecipientId = dto.RecipientId,
            };
    
            _dbContext.Conversations.Add(conversation);
            conversation.Id = await _dbContext.SaveChangesAsync();
        }
    
        var message = new MessageEntity
        {
            ConversationId = conversation.Id,
            SenderId = dto.InitiatorId,
            MessageContent = dto.MessageContent,
            SentAt = DateTime.UtcNow,
            Status = MessageStatus.Sent
        };
    
        _dbContext.Messages.Add(message);
        await _dbContext.SaveChangesAsync();
        
        return HandlerResult.Success();
    }
}