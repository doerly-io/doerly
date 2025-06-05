namespace Doerly.Module.Communication.Domain.Helpers;

public interface IUserOnlineStatusHelper
{
    Task<bool> IsUserOnlineAsync(int userId);
    Task MarkUserOnlineAsync(int userId);
    Task MarkUserOfflineAsync(int userId);
}