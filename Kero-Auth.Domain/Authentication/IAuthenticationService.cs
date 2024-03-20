namespace Kero_Auth.Domain.Authentication;
using User = User.User;

public interface IAuthenticationService
{
    Task<string?> SignUpAsync(User user, CancellationToken cancellationToken);
    Task<string?> LogInAsync(User user);
    Task<string> ResetPasswordAsync(string email);
    Task<string> GenerateVerificationUrlAsync(string email);
    Task<string> GeneratePasswordResetAsync(string email);
    Task<string> SignUpWithoutPasswordAsync(User user, CancellationToken cancellationToken);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> DeactivateUserAsync(Guid userId);
    Task<User?> GetUserAsync(Guid userId);
}
