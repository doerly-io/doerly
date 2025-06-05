using Doerly.Module.Communication.Domain.Helpers;
using Microsoft.Extensions.Caching.Distributed;

public class UserOnlineStatusHelper(IDistributedCache cache) : IUserOnlineStatusHelper
{
    private const string Prefix = "user-online:";

    public async Task MarkUserOnlineAsync(int userId)
    {
        try
        {
            await cache.SetStringAsync(Prefix + userId, "1", new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60)
            });
        }
        catch (Exception e)
        {
            Console.WriteLine($"Cache error: {e.Message}");
            throw;
        }
    }

    public async Task MarkUserOfflineAsync(int userId)
    {
        try
        {
            await cache.RemoveAsync(Prefix + userId);
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task<bool> IsUserOnlineAsync(int userId)
    {
        try
        {
            var value = await cache.GetStringAsync(Prefix + userId);
            return value != null;
        }
        catch (Exception e)
        {
            throw;
        }
    }
}