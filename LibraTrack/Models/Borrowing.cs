using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraTrack.Models;
public class Borrowing
{
    [Key]
    public int Borrowing_Id { get; set; }//pr key
    public DateTime Borrow_Date { get; set; } = DateTime.Now;
    public DateTime Due_Date { get; set; }
    public DateTime? Return_Date { get; set; }
    public int Copy_Id { get; set; } //fr key
    [ForeignKey("Copy_Id")]
    public Book_Copy? Copy { get; set; }
    public string User_Id { get; set; } = "";
    public ApplicationUser? User { get; set; }
    public string? ProcessedById { get; set; }
    public ApplicationUser? ProcessedBy { get; set; }
}
