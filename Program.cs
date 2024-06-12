using LibraryTask.Scripts;

namespace LibraryTask;

internal class Program
{
    // Команды меню
    private const string CommandAddUser = "1";
    private const string CommandListUsers = "2";
    private const string CommandAddBook = "3";
    private const string CommandListBooks = "4";
    private const string CommandLendBook = "5";
    private const string CommandReturnBook = "6";
    private const string CommandExit = "7";

    // Заголовки и промпты
    private const string MenuHeader = "Меню:";
    private const string MenuPrompt = "Введите номер действия:";
    private const string EnterUserName = "Введите имя пользователя:";
    private const string EnterBookTitle = "Введите название книги:";
    private const string EnterBookAuthor = "Введите автора книги:";
    private const string EnterBookId = "Введите Id книги:";
    private const string EnterUserId = "Введите Id пользователя:";

    // Сообщения об успехе
    private const string UserAdded = "Пользователь добавлен.";
    private const string BookAdded = "Книга добавлена.";
    private const string BookLent = "Книга выдана.";
    private const string BookReturned = "Книга возвращена.";

    // Сообщения об ошибках
    private const string ErrorUserNameEmpty = "Имя пользователя не может быть пустым.";
    private const string ErrorBookInfoEmpty = "Название или автор книги не могут быть пустыми.";
    private const string ErrorInvalidBookId = "Некорректный Id книги.";
    private const string ErrorInvalidUserId = "Некорректный Id пользователя.";
    private const string ErrorInvalidChoice = "Неверный выбор. Попробуйте еще раз.";
    private const string ErrorOccurred = "Ошибка: ";

    // Словарь команд и их описаний
    private static readonly Dictionary<string, string> ActionsByCommand = new()
    {
        { CommandAddUser, "Добавить пользователя" },
        { CommandListUsers, "Вывод списка пользователей" },
        { CommandAddBook, "Добавить книгу в библиотеку" },
        { CommandListBooks, "Вывод списка книг" },
        { CommandLendBook, "Выдать книгу пользователю" },
        { CommandReturnBook, "Вернуть книгу в библиотеку" },
        { CommandExit, "Выход" }
    };

    private static void Main(string[] args)
    {
        // Ручное управление зависимостями
        var bookRepository = new BookRepository();
        var userRepository = new UserRepository();
        var libraryService = new LibraryService(bookRepository, userRepository);

        while (true)
        {
            ShowMenu();
            string? choice = Console.ReadLine();

            if (SwitchMenu(choice, libraryService))
                return;

            Console.WriteLine();

            Console.WriteLine("Нажмите любую клавишу для продолжения.");
            Console.ReadKey();

            Console.Clear();
        }
    }

    private static bool SwitchMenu(string? choice, LibraryService libraryService)
    {
        switch (choice)
        {
            case CommandAddUser:
                AddUser(libraryService);
                break;

            case CommandListUsers:
                ShowAllUsers(libraryService);
                break;

            case CommandAddBook:
                AddBook(libraryService);
                break;

            case CommandListBooks:
                ShowAllBooks(libraryService);
                break;

            case CommandLendBook:
                LendBook(libraryService);
                break;

            case CommandReturnBook:
                ReturnBook(libraryService);
                break;

            case CommandExit:
                return true;

            default:
                Console.WriteLine(ErrorInvalidChoice);
                break;
        }

        return false;
    }

    private static void ShowMenu()
    {
        Console.WriteLine(MenuHeader);

        foreach (KeyValuePair<string, string> action in ActionsByCommand)
        {
            Console.WriteLine($"{action.Key}. {action.Value}");
        }

        Console.WriteLine(MenuPrompt);
    }

    private static void AddUser(ILibraryService libraryService)
    {
        Console.WriteLine(EnterUserName);
        string? name = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine(ErrorUserNameEmpty);
            return;
        }

        var user = new User(name);
        libraryService.AddUser(user);

        Console.WriteLine(UserAdded);
    }

    private static void ShowAllUsers(ILibraryService libraryService)
    {
        if (libraryService.CheckHasUsers())
            return;

        IEnumerable<User> users = libraryService.GetAllUsers();

        foreach (User user in users)
        {
            Console.WriteLine($"Id: {user.Id}, Name: {user.Name}");
        }
    }

    private static void AddBook(ILibraryService libraryService)
    {
        Console.WriteLine(EnterBookTitle);
        string? title = Console.ReadLine();

        Console.WriteLine(EnterBookAuthor);
        string? author = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(author))
        {
            Console.WriteLine(ErrorBookInfoEmpty);
            return;
        }

        var book = new Book(title, author);
        libraryService.AddBook(book);

        Console.WriteLine(BookAdded);
    }

    private static void ShowAllBooks(ILibraryService libraryService)
    {
        if (libraryService.CheckHasBooks())
            return;

        IEnumerable<Book> books = libraryService.GetAllBooks();

        foreach (Book book in books)
        {
            string userStatus = book.IsAvailable ? "Available" : $"Lent to User ID: {book.UserId}";
            Console.WriteLine($"Id: {book.Id}, Title: {book.Title}, Author: {book.Author}, Status: {userStatus}");
        }
    }

    private static void LendBook(ILibraryService libraryService)
    {
        if (libraryService.CheckHasUsers())
            return;

        if (libraryService.CheckHasBooks())
            return;
        
        ShowAllBooks(libraryService);

        Console.WriteLine(EnterBookId);

        if (int.TryParse(Console.ReadLine(), out int bookId) == false)
        {
            Console.WriteLine(ErrorInvalidBookId);
            return;
        }
        
        ShowAllUsers(libraryService);

        Console.WriteLine(EnterUserId);

        if (int.TryParse(Console.ReadLine(), out int userId) == false)
        {
            Console.WriteLine(ErrorInvalidUserId);
            return;
        }

        try
        {
            libraryService.LendBook(bookId, userId);
            Console.WriteLine(BookLent);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ErrorOccurred}{ex.Message}");
        }
    }

    private static void ReturnBook(ILibraryService libraryService)
    {
        if (libraryService.CheckHasBooks())
            return;

        if (libraryService.CheckHasUsers())
            return;
        
        ShowAllBooks(libraryService);

        Console.WriteLine(EnterBookId);

        if (int.TryParse(Console.ReadLine(), out int bookId) == false)
        {
            Console.WriteLine(ErrorInvalidBookId);
            return;
        }

        try
        {
            libraryService.ReturnBook(bookId);
            Console.WriteLine(BookReturned);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ErrorOccurred}{ex.Message}");
        }
    }
}