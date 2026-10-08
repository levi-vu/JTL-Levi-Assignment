namespace Modules.User.Domain.Users;

public sealed class User
{
    private User()
    {
        Username = null!;
    }

    private User(Guid id, Username username)
    {
        Id = id;
        Username = username;
    }

    public Guid Id { get; private set; }

    public Username Username { get; private set; }

    public static User Create(Username username)
    {
        ArgumentNullException.ThrowIfNull(username);
        return new User(Guid.NewGuid(), username);
    }
}
