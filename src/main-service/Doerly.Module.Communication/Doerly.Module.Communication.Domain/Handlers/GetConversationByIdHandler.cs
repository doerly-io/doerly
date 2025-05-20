using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Communication.Contracts.Dtos.Responses;
using Doerly.Module.Communication.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Communication.Domain.Handlers;

public class GetConversationByIdHandler(CommunicationDbContext dbContext) : BaseCommunicationHandler(dbContext)
{
    private readonly CommunicationDbContext _dbContext = dbContext;

    public async Task<HandlerResult<ConversationResponseDto>> HandleAsync(int conversationId)
    {
        var conversation = await _dbContext.Conversations
            .AsNoTracking()
            .Where(x => x.Id == conversationId)
            .Include(x => x.Messages)
            .FirstOrDefaultAsync();

        if (conversation == null)
        {
            return HandlerResult.Failure<ConversationResponseDto>(Resources.Get("Communication.ConversationNotFound"));
        }
        
        var messageDto = new ConversationResponseDto()
        {
            Id = conversation.Id,
            InitiatorId = conversation.InitiatorId,
            RecipientId = conversation.RecipientId,
            LastMessageId = conversation.Messages.Last().Id,
            Messages = conversation.Messages.Select(m => new MessageResponseDto
            {
                Id = m.Id,
                SenderId = m.SenderId,
                ConversationId = m.ConversationId,
                MessageContent = m.MessageContent,
                SentAt = m.SentAt,
                Status = m.Status
            }).ToList()
        };

        return HandlerResult.Success(messageDto);
    }
}