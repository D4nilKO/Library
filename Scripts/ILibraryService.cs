namespace LibraryTask.Scripts;

public interface ILibraryService
{
    void AddBook(Book book);
    void AddUser(User user);
    void LendBook(int bookId, int userId);
    void ReturnBook(int bookId);
    
    IEnumerable<User> GetAllUsers();
    IEnumerable<Book> GetAllBooks();
    
    bool CheckHasBooks();
    bool CheckHasUsers();
}