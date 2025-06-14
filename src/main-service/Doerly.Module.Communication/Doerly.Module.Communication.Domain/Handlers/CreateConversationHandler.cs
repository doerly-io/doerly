using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Communication.DataTransferObjects.Requests;
using Doerly.Module.Communication.DataAccess;
using Doerly.Module.Communication.DataAccess.Entities;
using Doerly.Proxy.Profile;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Communication.Domain.Handlers;

public class CreateConversationHandler(CommunicationDbContext dbContext, IProfileModuleProxy profileModule) : BaseCommunicationHandler(dbContext)
{
    public async Task<OperationResult<int>> HandleAsync(CreateConversationRequest dto, int initiatorId)
    {
        if (dto.RecipientId == initiatorId)
        {
            return OperationResult.Failure<int>(Resources.Get("Communication.CannotCreateConversationWithSelf"));
        }
        
        var existingConversation = await dbContext.Conversations
            .FirstOrDefaultAsync(c =>
                (c.InitiatorId == initiatorId && c.RecipientId == dto.RecipientId) ||
                (c.InitiatorId == dto.RecipientId && c.RecipientId == initiatorId));

        if (existingConversation != null)
        {
            return OperationResult.Success(existingConversation.Id);
        }

        var conversation = new ConversationEntity
        {
            InitiatorId = initiatorId,
            RecipientId = dto.RecipientId
        };

        dbContext.Conversations.Add(conversation);
        await dbContext.SaveChangesAsync();

        return OperationResult.Success(conversation.Id);
    }
}