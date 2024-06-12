namespace LibraryTask.Scripts;

public class UserRepository : IUserRepository
{
    private readonly List<User> _users = new();

    public void Add(User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        _users.Add(user);
    }

    public IEnumerable<User> GetAll()
    {
        return _users;
    }

    public User GetById(int id)
    {
        return _users.SingleOrDefault(u => u.Id == id);
    }
}