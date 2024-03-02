namespace Kero_Auth.Infrastructure.Authentication;

using Firebase.Auth;
using Kero_Auth.Domain.Authentication;
using Kero_Auth.Domain.Authentication.Exceptions;
using Kero_Auth.Infrastructure.Services.Options;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using User = Domain.User.User;

public class AuthenticationService : IAuthenticationService
{
    private readonly FirebaseAuthClient _firebaseAuth;
    private readonly NoReplyOptions _noReplyOptions;
    public AuthenticationService(FirebaseAuthClient firebaseAuth, IOptions<NoReplyOptions> noReplyOptions)
    {
        _noReplyOptions = noReplyOptions.Value;
        _firebaseAuth = firebaseAuth;
    }

    public async Task<string?> SignUpAsync(User user)
    {
        var userCredentials = await _firebaseAuth.CreateUserWithEmailAndPasswordAsync(user.Email, user.Password);
        return userCredentials.User.Uid;
    }

    public async Task<string?> LogInAsync(User user)
    {
        var userCredentials = await _firebaseAuth.SignInWithEmailAndPasswordAsync(user.Email, user.Password);
        return userCredentials is null ? null : await userCredentials.User.GetIdTokenAsync();
    }

    public async Task<string> ResetPasswordAsync(string email)
    {
        var providers = await _firebaseAuth.FetchSignInMethodsForEmailAsync(email);
        if (!providers.UserExists)
        {
            throw new UserNotFoundException();
        }
        await _firebaseAuth.ResetEmailPasswordAsync(email);
        return _noReplyOptions.Email;
    }
}
