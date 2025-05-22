namespace Doerly.Module.Communication.Domain.Hubs;

public interface ICommunicationHub
{
    Task JoinConversation(string conversationId);

    Task SendMessage(string senderId, string messageContent);

    Task LeaveConversation(string conversationId);

    Task UserTyping(string senderId);
    
    Task MessageRead(int messageId, string senderId);
    
    Task UserStatusChanged(string userId, bool isOnline);
}