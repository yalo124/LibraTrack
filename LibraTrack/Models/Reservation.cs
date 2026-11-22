using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace LibraTrack.Models;

public enum ReservationStatus
{
    Pending, //waiting 
    Approved, //ready for pickup
    OnHold, // copy is held for this user until expiryDate
    Completed, 
    Cancelled, 
    Expiried // hold ran out
}
public class Reservation
{
    [Key]
    public int ReservationId { get; set; }
    public DateTime ReservedDate { get; set; } = DateTime.Now;
    public DateTime? ExpiryDate { get; set; }
    public int? QueuePosition { get; set; }

    public ReservationStatus Status { get; set; } = ReservationStatus.Pending;

    public int BookId { get; set; }
    public Book? Book { get; set; }

    public string UserId { get; set; } = "";
    public ApplicationUser? User { get; set; }
}
