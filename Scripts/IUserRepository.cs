namespace LibraryTask.Scripts;

public interface IUserRepository
{
    void Add(User user);
    IEnumerable<User> GetAll();
    User GetById(int id);
}