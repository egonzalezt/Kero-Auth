namespace Kero_Auth.Workers.Exceptions;

using Domain.SharedKernel;

public class HeaderNotFoundException : BusinessException
{
    public HeaderNotFoundException() : base()
    {
    }

    public HeaderNotFoundException(string message) : base($"Header {message} not found")
    {
    }

    public HeaderNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
