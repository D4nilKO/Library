namespace LibraryTask.Scripts;

public class User
{
    private static int s_nextId = 1;
    
    public int Id { get; }
    public string Name { get; }

    public User(string name)
    {
        Id = s_nextId++;
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }
}