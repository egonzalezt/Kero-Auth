namespace Kero_Auth.Domain.Authentication;
using Kero_Auth.Domain.User;

public interface IAuthenticationService
{
    Task<string> RegisterAsync(User user);
}
