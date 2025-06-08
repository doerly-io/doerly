using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Communication.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Communication.Domain.Handlers;

public class CheckConversationExistsHandler(CommunicationDbContext dbContext) : BaseCommunicationHandler(dbContext)
{
    private readonly CommunicationDbContext _dbContext = dbContext;

    public async Task<HandlerResult<int?>> HandleAsync(int initiatorId, int recipientId)
    {
        var conversation = await _dbContext.Conversations
            .FirstOrDefaultAsync(c =>
                (c.InitiatorId == initiatorId && c.RecipientId == recipientId) ||
                (c.InitiatorId == recipientId && c.RecipientId == initiatorId));

        return HandlerResult.Success(conversation?.Id);
    }
}