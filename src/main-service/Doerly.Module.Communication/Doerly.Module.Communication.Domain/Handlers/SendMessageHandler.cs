using Doerly.Domain.Models;
using Doerly.Module.Communication.Contracts.Dtos.Requests;
using Doerly.Module.Communication.DataAccess;
using Doerly.Module.Communication.DataAccess.Entities;
using Doerly.Module.Communication.Domain.Hubs;
using Doerly.Module.Communication.Enums;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Communication.Domain.Handlers;

public class SendMessageHandler(CommunicationDbContext dbContext, IHubContext<CommunicationHub, ICommunicationHub> hubContext) : BaseCommunicationHandler(dbContext)
{
    private readonly CommunicationDbContext _dbContext = dbContext;

    public async Task<HandlerResult> HandleAsync(SendMessageRequest dto)
    {
        var conversation = await _dbContext.Conversations.FirstOrDefaultAsync(c => c.InitiatorId == dto.InitiatorId && c.RecipientId == dto.RecipientId);
    
        if (conversation == null)
        {
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
        
        await hubContext.Clients.Group(conversation.Id.ToString()).SendMessage(message.SenderId.ToString(), message.MessageContent);
        
        return HandlerResult.Success();
    }
}