using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace LibraTrack.Models;

public enum Notification_Type
{
    Reminder, Overdue, Reservation
}
public class Notification
{
    [Key]
    public int NotificationId { get; set; }// pr key

    [Required, StringLength(600)]
    public string Message { get; set; } = "";
    public Notification_Type Type { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public bool IsRead { get; set; }

    public string UserId { get; set; } = "";
    public ApplicationUser? User { get; set; }
}
