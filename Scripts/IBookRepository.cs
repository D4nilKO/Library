namespace LibraryTask.Scripts;

public interface IBookRepository
{
    Book GetById(int id);
    IEnumerable<Book> GetAll();
    void Add(Book book);
    void Update(Book book);
}