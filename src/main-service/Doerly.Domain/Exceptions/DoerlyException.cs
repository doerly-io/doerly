namespace Doerly.Domain.Exceptions;

public class DoerlyException : Exception
{
    public DoerlyException()
    {
    }

    public DoerlyException(string message) : base(message)
    {
    }

    public DoerlyException(string message, params object[] args) : base(string.Format(message, args))
    {
        
    }
}
