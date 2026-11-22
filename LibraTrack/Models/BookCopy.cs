using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraTrack.Models;

public enum CopyStatus
{
    Available, Borrowed, Reserved
}

public class BookCopy
{
    [Key]
    public int CopyId { get; set; }// primary key

    [Required, StringLength(50)] public string Barcode { get; set; } = "";
    [StringLength(150)] public string? Branch { get; set; }
    public CopyStatus Status { get; set; } = CopyStatus.Available;
    public bool IsDeleted { get; set; }
    public int BookId { get; set; }// foreign key
    public Book? Book { get; set; }
}
