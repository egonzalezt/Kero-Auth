namespace Kero_Auth.Infrastructure.Authentication;

using Firebase.Auth;
using Kero_Auth.Domain.Authentication;
using System.Threading.Tasks;
using User = Domain.User.User;

public class AuthenticationService : IAuthenticationService
{
    private readonly FirebaseAuthClient _firebaseAuth;
    public AuthenticationService(FirebaseAuthClient firebaseAuth)
    {
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
}
