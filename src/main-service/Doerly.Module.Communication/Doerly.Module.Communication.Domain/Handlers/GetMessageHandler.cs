using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Communication.Contracts.Dtos.Requests;
using Doerly.Module.Communication.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Communication.Domain.Handlers;

public class GetMessageHandler(CommunicationDbContext dbContext) : BaseCommunicationHandler(dbContext)
{
    private readonly CommunicationDbContext _dbContext = dbContext;

    public async Task<HandlerResult<MessageDto>> HandleAsync(int messageId)
    {
        var message = await _dbContext.Messages
            .AsNoTracking()
            .Where(x => x.Id == messageId)
            .FirstOrDefaultAsync();

        //TODO: MessageNotFound resources 
        if (message == null)
            return HandlerResult.Failure<MessageDto>(Resources.Get("MessageNotFound"));
        
        var messageDto = new MessageDto()
        {
            Id = message.Id,
            SenderId = message.SenderId,
            ConversationId = message.ConversationId,
            //TODO: Conversation = message.Conversation,
            MessageContent = message.MessageContent,
            SentAt = message.SentAt,
            Status = message.Status
        };

        return HandlerResult.Success(messageDto);
    }
}