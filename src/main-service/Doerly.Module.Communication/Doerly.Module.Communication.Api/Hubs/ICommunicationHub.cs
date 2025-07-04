using Doerly.Module.Communication.DataTransferObjects.Responses;

namespace Doerly.Module.Communication.Domain.Hubs;

public interface ICommunicationHub
{
    Task UserJoined(string conversationId);

    Task ReceiveMessage(MessageResponse message);

    Task LeaveConversation(string conversationId);

    Task UserTyping(string senderId);
    
    Task MessageRead(int messageId, string senderId);
    
    Task MessageDelivered(int messageId, string senderId);
    
    Task MarkMessagesAsRead(int[] messageIds, string senderId);
    
    Task MarkMessagesAsDelivered(int[] messageIds, string senderId);
    
    Task UserStatusChanged(string userId, bool isOnline);

    Task UpdateLastMessage(MessageResponse conversation);
}