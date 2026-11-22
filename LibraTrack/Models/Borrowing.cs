using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraTrack.Models;
public class Borrowing
{
    [Key]
    public int BorrowingId { get; set; }//pr key
    public DateTime BorrowDate { get; set; } = DateTime.Now;
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public int Copy_Id { get; set; } //fr key
    public BookCopy? Copy { get; set; }
    public string UserId { get; set; } = "";
    public ApplicationUser? User { get; set; }
    public string? ProcessedById { get; set; }
    public ApplicationUser? ProcessedBy { get; set; }
}
