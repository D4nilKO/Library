namespace LibraryTask.Scripts;

public class LibraryService : ILibraryService
{
    private const string ErrorNoUsers = "Нет пользователей.";
    private const string ErrorNoBooks = "Нет книг в библиотеке.";

    private readonly IBookRepository _bookRepository;
    private readonly IUserRepository _userRepository;

    public LibraryService(IBookRepository bookRepository, IUserRepository userRepository)
    {
        _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public void AddBook(Book book)
    {
        if (book == null) throw new ArgumentNullException(nameof(book));
        _bookRepository.Add(book);
    }

    public IEnumerable<Book> SearchBooks(string title, string author)
    {
        return _bookRepository.GetAll().Where(b =>
            (string.IsNullOrEmpty(title) || b.Title.Contains(title, StringComparison.OrdinalIgnoreCase)) &&
            (string.IsNullOrEmpty(author) || b.Author.Contains(author, StringComparison.OrdinalIgnoreCase)));
    }

    public void AddUser(User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        _userRepository.Add(user);
    }

    public IEnumerable<User> GetAllUsers()
    {
        return _userRepository.GetAll();
    }

    public void LendBook(int bookId, int userId)
    {
        Book book = _bookRepository.GetById(bookId) ?? throw new InvalidOperationException("Book not found.");
        User user = _userRepository.GetById(userId) ?? throw new InvalidOperationException("User not found.");

        if (book.IsAvailable == false)
            throw new InvalidOperationException("Book is not available.");

        book.LendTo(userId);
        _bookRepository.Update(book);
    }

    public IEnumerable<Book> GetAllBooks()
    {
        return _bookRepository.GetAll();
    }

    public bool CheckHasBooks()
    {
        if (GetAllBooks().Any() == false)
        {
            Console.WriteLine(ErrorNoBooks);
            return true;
        }

        return false;
    }

    public bool CheckHasUsers()
    {
        if (GetAllUsers().Any() == false)
        {
            Console.WriteLine(ErrorNoUsers);
            return true;
        }

        return false;
    }

    public void ReturnBook(int bookId)
    {
        Book book = _bookRepository.GetById(bookId) ?? throw new InvalidOperationException("Book not found.");

        if (book.IsAvailable)
            throw new InvalidOperationException("Book is already available.");

        book.ReturnBook();
        _bookRepository.Update(book);
    }
}