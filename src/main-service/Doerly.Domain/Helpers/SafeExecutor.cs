using Doerly.Domain.Models;

namespace Doerly.Domain.Helpers;

public static class SafeExecutor
{
    public static HandlerResult<TResult> Execute<TResult>(Func<TResult> func)
    {
        try
        {
            var result = func();
            return HandlerResult.Success(result);
        }
        catch (Exception ex)
        {
            return HandlerResult.Failure<TResult>(ex.ToString());
        }
        
    }
    
}
