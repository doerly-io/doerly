using Doerly.Domain.Models;
using Doerly.Module.Communication.Contracts.Dtos.Requests;
using Doerly.Module.Communication.DataAccess;
using Doerly.Module.Communication.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Communication.Domain.Handlers;

public class CreateConversationHandler(CommunicationDbContext dbContext) : BaseCommunicationHandler(dbContext)
{
    public async Task<HandlerResult<int>> HandleAsync(CreateConversationRequest dto, int initiatorId)
    {
        var existingConversation = await dbContext.Conversations
            .FirstOrDefaultAsync(c =>
                (c.InitiatorId == initiatorId && c.RecipientId == dto.RecipientId) ||
                (c.InitiatorId == dto.RecipientId && c.RecipientId == initiatorId));

        if (existingConversation != null)
        {
            return HandlerResult.Success(existingConversation.Id);
        }

        var conversation = new ConversationEntity
        {
            InitiatorId = initiatorId,
            RecipientId = dto.RecipientId
        };

        dbContext.Conversations.Add(conversation);
        await dbContext.SaveChangesAsync();

        return HandlerResult.Success(conversation.Id);
    }
}