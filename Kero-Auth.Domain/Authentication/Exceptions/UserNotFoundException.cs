namespace Kero_Auth.Domain.Authentication.Exceptions;

using SharedKernel;

public class UserNotFoundException : BusinessException
{
    public UserNotFoundException() : base("User not found")
    {
    }

    public UserNotFoundException(string message) : base(message)
    {
    }

    public UserNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}