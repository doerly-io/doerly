using Doerly.Domain.Models;

namespace Doerly.Common.Helpers;

public static class SafeExecutor
{
    public static OperationResult<TResult> Execute<TResult>(Func<TResult> func)
    {
        try
        {
            var result = func();
            return OperationResult.Success(result);
        }
        catch (Exception ex)
        {
            return OperationResult.Failure<TResult>(ex.ToString());
        }
        
    }
    
}
