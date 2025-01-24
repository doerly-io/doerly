namespace Doerly.Domain.Models;

public class HandlerResult
{
    public bool IsSuccess { get; init; }
    public string ErrorMessage { get; init; }

    protected HandlerResult(bool isSuccess, string errorMessage)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    public static HandlerResult Success() => new(true, string.Empty);
    public static HandlerResult Failure(string errorMessage) => new(false, errorMessage);

    public static HandlerResult<T> Success<T>(T value) => new HandlerResult<T>(true, value, string.Empty);

    public static HandlerResult<T> Failure<T>(string errorMessage) => new HandlerResult<T>(false, default, errorMessage);
}

public class HandlerResult<T> : HandlerResult
{
    public T Value { get; init; }

    public HandlerResult(bool isSuccess, T value, string errorMessage) : base(isSuccess, errorMessage)
    {
        Value = value;
    }
    
    
}
