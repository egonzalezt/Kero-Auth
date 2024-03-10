namespace Kero_Auth.Domain.User;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string? Password { get; private set; }

    private User(string email, string password)
    {
        Email = email;
        Password = password;
    }

    private User(string email)
    {
        Email = email;
    }

    public void SetId(Guid id)
    {
        Id = id;
    }

    public static User Build(string email, string password)
    {
        return new User(email, password);
    }

    public static User Build(string email)
    {
        return new User(email);
    }
}
