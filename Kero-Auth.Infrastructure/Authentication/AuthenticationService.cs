namespace Kero_Auth.Infrastructure.Authentication;

using FirebaseAdmin.Auth;
using Kero_Auth.Domain.Authentication;
using Kero_Auth.Domain.User;
using System.Threading.Tasks;

public class AuthenticationService : IAuthenticationService
{
    public async Task<string> RegisterAsync(User user)
    {
        var userArgs = new UserRecordArgs
        {
            Email = user.Email,
            Password = user.Password
        };

        var userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(userArgs);
        return userRecord.Uid;
    }
}
