namespace Kero_Auth.Domain.User;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string? Password { get; private set; }
    public string? Name { get; private set; }
    public bool Disabled { get; private set; } = false;
    private User(string email, string password, string name)
    {
        Email = email;
        Password = password;
        Name = name;
    }

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

    public void SetDisabled(bool disabled)
    {
        Disabled = disabled;
    }

    public void SetName(string? name)
    {
        if (name is null)
        {
            return;
        }

        string[] nameParts = name.Split(' ');
        if (nameParts.Length > 0)
        {
            Name = nameParts[0];
        }
        else
        {
            Name = "";
        }
    }


    public static User Build(string email, string password)
    {
        return new User(email, password);
    }

    public static User Build(string email, string password, string name)
    {
        return new User(email, password, name);
    }

    public static User Build(string email)
    {
        return new User(email);
    }
}
