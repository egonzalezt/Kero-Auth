namespace Kero_Auth.Infrastructure.Authentication;

using Firebase.Auth;
using FirebaseAdmin.Auth;
using Kero_Auth.Domain.Authentication;
using Kero_Auth.Infrastructure.Services.Options;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using User = Domain.User.User;

public class AuthenticationService : IAuthenticationService
{
    private readonly FirebaseAuthClient _firebaseAuthClient;
    private readonly NoReplyOptions _noReplyOptions;
    private readonly FirebaseAuth _firebaseAuth;

    public AuthenticationService(FirebaseAuth firebaseAuth,FirebaseAuthClient firebaseAuthClient, IOptions<NoReplyOptions> noReplyOptions)
    {
        _noReplyOptions = noReplyOptions.Value;
        _firebaseAuthClient = firebaseAuthClient;
        _firebaseAuth = firebaseAuth;
    }

    public async Task<string?> SignUpAsync(User user)
    {
        var userCredentials = await _firebaseAuth.CreateUserAsync(new UserRecordArgs() { Email = user.Email, Password = user.Password });
        return userCredentials.Uid;
    }

    public async Task<string?> LogInAsync(User user)
    {
        var userCredentials = await _firebaseAuthClient.SignInWithEmailAndPasswordAsync(user.Email, user.Password);
        return userCredentials is null ? null : await userCredentials.User.GetIdTokenAsync();
    }

    public async Task<string> ResetPasswordAsync(string email)
    {
        await _firebaseAuth.GetUserByEmailAsync(email);
        await _firebaseAuthClient.ResetEmailPasswordAsync(email);
        return _noReplyOptions.Email;
    }

    public async Task<string> GenerateVerificationUrlAsync(string email)
    {
        return await _firebaseAuth.GenerateEmailVerificationLinkAsync(email);
    }

    public async Task<string> GeneratePasswordResetAsync(string email)
    {
        return await _firebaseAuth.GeneratePasswordResetLinkAsync(email);
    }

    public async Task<string> SignUpWithoutPasswordAsync(string email)
    {
        var userCredentials = await _firebaseAuth.CreateUserAsync(new UserRecordArgs { Email = email });
        return userCredentials.Uid;
    }
}
