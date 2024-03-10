namespace Kero_Auth.Workers.Exceptions;

using Domain.SharedKernel;

public class InvalidEventTypeException : BusinessException
{
    public InvalidEventTypeException() : base()
    {
    }

    public InvalidEventTypeException(string message) : base($"EventType {message} not supported")
    {
    }

    public InvalidEventTypeException(string message, Exception innerException) : base(message, innerException)
    {
    }
}