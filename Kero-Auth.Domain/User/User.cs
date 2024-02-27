namespace Kero_Auth.Domain.User;

public class User
{
    public string Id { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }

    private User(string email, string password)
    {
        Email = email;
        Password = password;
    }

    public void SetId(string id)
    {
        Id = id;
    }

    public static User Build(string email, string password)
    {
        return new User(email, password);
    }
}
