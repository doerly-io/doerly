using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Communication.DataAccess;
using Doerly.Module.Communication.Enums;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Communication.Domain.Handlers;

public class MarkMessageStatusHandler(CommunicationDbContext dbContext) : BaseCommunicationHandler(dbContext)
{
    private readonly CommunicationDbContext _dbContext = dbContext;

    public async Task HandleAsync(int messageId, EMessageStatus status)
    {
        var message = await _dbContext.Messages
            .FirstOrDefaultAsync(m => m.Id == messageId);
        
        if (message == null)
        {
            HandlerResult.Failure<int>(Resources.Get("Communication.MessageNotFound"));
            return;
        }
        
        message.Status = status;
        await _dbContext.SaveChangesAsync();

        HandlerResult.Success();
    }

    public async Task HandleAsync(int[] messageIds, EMessageStatus status)
    {
        var messages = await _dbContext.Messages
            .Where(m => messageIds.Contains(m.Id))
            .ToListAsync();

        if (!messages.Any())
        {
            HandlerResult.Failure<int>(Resources.Get("Communication.MessageNotFound"));
            return;
        }

        foreach (var message in messages)
        {
            message.Status = status;
        }

        await _dbContext.SaveChangesAsync();
        HandlerResult.Success();
    }
}