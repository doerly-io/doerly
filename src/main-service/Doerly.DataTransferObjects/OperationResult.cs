namespace Doerly.Domain.Models;

public class OperationResult
{
    public bool IsSuccess { get; init; }
    public string ErrorMessage { get; init; }

    protected OperationResult(bool isSuccess, string errorMessage)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    public static OperationResult Success() => new(true, string.Empty);
    public static OperationResult Failure(string errorMessage) => new(false, errorMessage);

    public static OperationResult<T> Success<T>(T value) => new OperationResult<T>(true, value, string.Empty);

    public static OperationResult<T> Failure<T>(string errorMessage) => new OperationResult<T>(false, default, errorMessage);

    public static OperationResult<T> Failure<T>(T value) => new OperationResult<T>(false, value);
}

public class OperationResult<T> : OperationResult
{
    public T Value { get; init; }

    public OperationResult(bool isSuccess, string errorMessage) : base(isSuccess, errorMessage)
    {
        Value = default;
    }

    public OperationResult(bool isSuccess, T value) : base(isSuccess, string.Empty)
    {
        Value = default;
    }

    public OperationResult(bool isSuccess, T value, string errorMessage) : base(isSuccess, errorMessage)
    {
        Value = value;
    }
}
