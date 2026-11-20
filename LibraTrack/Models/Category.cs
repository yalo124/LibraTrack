using System.ComponentModel.DataAnnotations;

namespace LibraTrack.Models;
public class Category
{
    [Key]
    public int Category_Id { get; set; }

    [Required, StringLength(150)] public string Name { get; set; } = "";

    [StringLength(400)] public string? Description { get; set; }

    public List<Book> Books { get; set; } = new();
}
