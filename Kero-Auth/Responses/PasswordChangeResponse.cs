namespace Kero_Auth.Responses;

public class PasswordChangeResponse
{
    public string Message { get; set; }
    public string Email {  get; set; }

    public static PasswordChangeResponse GenerateMessage(string email)
    {
        var message = $"Kero request the change of your password, you will receive a message from {email}";
        return new PasswordChangeResponse
        {
            Message = message,
            Email = email
        };
    }
}
