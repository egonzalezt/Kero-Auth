namespace Kero_Auth.Domain.User.Dtos;

public class UserSignInUriDto
{
    public Guid Id { get; set; }
    public string PasswordSetUri { get; set; }
    public string VerificationEmailUri { get; set; }
}
