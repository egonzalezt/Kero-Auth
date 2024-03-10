namespace Kero_Auth.Workers.Exceptions;

using Domain.SharedKernel;

public class InvalidBodyException : BusinessException
{
    public InvalidBodyException() : base()
    {
    }

    public InvalidBodyException(string message) : base(message)
    {
    }

    public InvalidBodyException(string message, Exception innerException) : base(message, innerException)
    {
    }
}