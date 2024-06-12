namespace LibraryTask.Scripts;

public class BookRepository : IBookRepository
{
    private readonly List<Book> _books = new();

    public void Add(Book book)
    {
        if (book == null) throw new ArgumentNullException(nameof(book));
        _books.Add(book);
    }

    public IEnumerable<Book> GetAll()
    {
        return _books;
    }

    public Book GetById(int id)
    {
        return _books.SingleOrDefault(b => b.Id == id);
    }

    public void Update(Book book)
    {
        Book? existingBook = GetById(book.Id);

        if (existingBook == null)
            throw new InvalidOperationException("Book not found.");

        if (book.UserId.HasValue && existingBook.IsAvailable)
        {
            existingBook.LendTo(book.UserId.Value);
            return;
        }

        if (book.UserId.HasValue == false && existingBook.IsAvailable == false)
        {
            existingBook.ReturnBook();
        }
    }
}