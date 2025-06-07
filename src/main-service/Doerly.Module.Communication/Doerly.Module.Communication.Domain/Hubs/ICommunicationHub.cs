using Doerly.Module.Communication.Contracts.Dtos.Responses;

namespace Doerly.Module.Communication.Domain.Hubs;

public interface ICommunicationHub
{
    Task UserJoined(string conversationId);

    Task ReceiveMessage(MessageResponseDto message);

    Task LeaveConversation(string conversationId);

    Task UserTyping(string senderId);
    
    Task MessageRead(int messageId, string senderId);
    
    Task MessageDelivered(int messageId, string senderId);
    
    Task MarkMessagesAsRead(int[] messageIds, string senderId);
    
    Task MarkMessagesAsDelivered(int[] messageIds, string senderId);
    
    Task UserStatusChanged(string userId, bool isOnline);

    Task UpdateLastMessage(MessageResponseDto conversation);
}