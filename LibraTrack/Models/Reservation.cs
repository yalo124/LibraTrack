using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace LibraTrack.Models;

public enum Reservation_Status
{
    Pending, //waiting 
    Approved, //ready for pickup
    OnHold, // copy is held for this user until expiryDate
    Completed, 
    Cancelled, 
    Exprired // hold ran out
}
public class Reservation
{
    [Key]
    public int Reservation_Id { get; set; }
    public DateTime Reserved_Date { get; set; } = DateTime.Now;
    public DateTime? Expiry_Date { get; set; }
    public int? Queue_Position { get; set; }

    public Reservation_Status Status { get; set; } = Reservation_Status.Pending;

    public int Book_Id { get; set; }
    public Book? Book { get; set; }

    public string User_Id { get; set; } = "";
    public ApplicationUser? User { get; set; }
}
