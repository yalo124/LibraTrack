using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraTrack.Models;
public class Book
{
    [Key]public int BookId { get; set; }//primary key

    [Required, StringLength(250)]public string Title { get; set; } = "";   
    
    [Required, StringLength(150)] public string Author { get; set; } = "";
    [Required, StringLength(20)] public string ISBN { get; set; } = "";
    public string? Publisher { get; set; }
    public int? PublicationYear { get; set; }
    public string? Description { get; set; }
    public string? CoverImageUrl { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsDeleted { get; set; }
    public int CategoryId { get; set; }

    public Category? Category { get; set; } //foreign key
    public List<BookCopy> Copies { get; set; } = new();
}
