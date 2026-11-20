using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraTrack.Models;
public class Book
{
    public int BookId { get; set; }

    [Required, StringLength(250)] [Key] 
    public string Title { get; set; } = ""; //primary key
    [Required, StringLength(150)] public string Author { get; set; } = "";
    [Required, StringLength(20)] public string ISBN { get; set; } = "";
    public string? Publisher { get; set; }
    public int? Publication_Year { get; set; }
    public string? Description { get; set; }
    public string? Cover_Image_Url { get; set; }
    public bool Is_Featured { get; set; }
    public bool Is_Deleted { get; set; }
    public int Category_Id { get; set; }

    [ForeignKey("Category_Id")]
    public Category? Category { get; set; } //foreign key
    public List<Book_Copy> Copies { get; set; } = new();
}
