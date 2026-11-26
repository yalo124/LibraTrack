namespace LibraTrack.Models;


public class StudentResult
{
    public string Id { get; set; } = "";
    public string FullName { get; set; } = "";
    public string? SchoolId { get; set; }
    public string? Email { get; set; }
}

public class BookResult
{
    public int BookId { get; set; }
    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
    public string ISBN { get; set; } = "";
    public int AvailableCopies { get; set; }
}


public class QuickSearchViewModel
{
    public string? Query { get; set; }
    public List<StudentResult> Students { get; set; } = new();
    public List<BookResult> Books { get; set; } = new();
}

