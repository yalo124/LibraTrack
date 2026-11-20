using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraTrack.Models;

public enum Copy_Status
{
    Available, Borrowed, Reserved
}

public class Book_Copy
{
    [Key]
    public int Copy_Id { get; set; }// primary key

    [Required, StringLength(50)] public string Barcode { get; set; } = "";
    [StringLength(150)] public string? Branch { get; set; }
    public Copy_Status Status { get; set; } = Copy_Status.Available;
    public bool Is_Deleted { get; set; }
    public int Book_Id { get; set; }// foreign key
    public Book? Book { get; set; }
}
