using System.Security.Cryptography.Pkcs;

namespace LibraTrack.Models;

public class SearchResultViewModel
{
    public List<Book> Books { get; set; } = new();
    public List<Category> Categories { get; set; } = new();

    public string? Keyword { get; set; }
    public int? CategoryId { get; set; }

    public int Page { get; set; } = 1;
    public int TotalPages { get; set; }
    public int TotalResults { get; set; }
}

