using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Communication.Contracts.Dtos.Responses;
using Doerly.Module.Communication.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Communication.Domain.Handlers;

public class GetMessageByIdHandler(CommunicationDbContext dbContext) : BaseCommunicationHandler(dbContext)
{
    private readonly CommunicationDbContext _dbContext = dbContext;

    public async Task<HandlerResult<MessageResponseDto>> HandleAsync(int messageId)
    {
        var message = await _dbContext.Messages
            .AsNoTracking()
            .Where(x => x.Id == messageId)
            .Include(x => x.Conversation)
            .FirstOrDefaultAsync();

        if (message == null)
            return HandlerResult.Failure<MessageResponseDto>(Resources.Get("Communication.MessageNotFound"));
        
        var messageDto = new MessageResponseDto()
        {
            Id = message.Id,
            SenderId = message.SenderId,
            ConversationId = message.ConversationId,
            Conversation = new ConversationHeaderResponseDto
            {
                Id = message.Conversation.Id,
                InitiatorId = message.Conversation.InitiatorId,
                RecipientId = message.Conversation.RecipientId,
            },
            MessageContent = message.MessageContent,
            SentAt = message.SentAt,
            Status = message.Status
        };

        return HandlerResult.Success(messageDto);
    }
}