using Doerly.Module.Communication.DataAccess;

namespace Doerly.Module.Communication.Domain.Handlers;

public class SendMessageHandler : BaseCommunicationHandler
{
    public SendMessageHandler(CommunicationDbContext dbContext) : base(dbContext)
    {
    }
    
    // public async Task<HandlerResult> HandleAsync(SendMessageDto dto)
    // {
    //     var conversation = await _context.Conversations
    //         .FirstOrDefaultAsync(c => 
    //             (c.User1Id == senderId && c.User2Id == receiverId) ||
    //             (c.User1Id == receiverId && c.User2Id == senderId));
    //
    //     if (conversation == null)
    //     {
    //         conversation = new ConversationEntity
    //         {
    //             User1Id = senderId,
    //             User2Id = receiverId
    //         };
    //
    //         _context.Conversations.Add(conversation);
    //         await _context.SaveChangesAsync();
    //     }
    //
    //     var message = new MessageEntity
    //     {
    //         ConversationId = conversation.Id,
    //         SenderId = senderId,
    //         MessageContent = messageContent,
    //         SentAt = DateTime.UtcNow,
    //         Status = MessageStatus.Sent
    //     };
    //
    //     _context.Messages.Add(message);
    //     await _context.SaveChangesAsync();
    //     
    //     return HandlerResult.Success();
    // }
}