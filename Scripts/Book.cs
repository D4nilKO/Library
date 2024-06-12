namespace LibraryTask.Scripts;

public class Book
{
    private static int s_nextId = 1;
    
    public int Id { get; }
    public string Title { get; }
    public string Author { get; }
    public bool IsAvailable { get; private set; } = true;
    public int? UserId { get; private set; }

    public Book(string title, string author)
    {
        Id = s_nextId++;
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Author = author ?? throw new ArgumentNullException(nameof(author));
    }

    public void LendTo(int userId)
    {
        if (IsAvailable == false) 
            throw new InvalidOperationException("Book is already lent out.");
        
        IsAvailable = false;
        UserId = userId;
    }

    public void ReturnBook()
    {
        if (IsAvailable) 
            throw new InvalidOperationException("Book is already available.");
        
        IsAvailable = true;
        UserId = null;
    }
}