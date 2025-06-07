using Doerly.Module.Communication.Contracts.Responses;

namespace Doerly.Module.Communication.Domain.Hubs;

public interface ICommunicationHub
{
    Task UserJoined(string conversationId);

    Task ReceiveMessage(MessageResponseDto message);

    Task LeaveConversation(string conversationId);

    Task UserTyping(string senderId);
    
    Task MessageRead(int messageId, string senderId);
    
    Task UserStatusChanged(string userId, bool isOnline);
}