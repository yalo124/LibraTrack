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
    public int Notifiation_Id { get; set; }// pr key

    [Required, StringLength(600)]
    public string Message { get; set; } = "";
    public Notification_Type Type { get; set; }
    public DateTime Created_Date { get; set; } = DateTime.Now;
    public bool Is_Read { get; set; }

    public string User_Id { get; set; } = "";
    public ApplicationUser? User { get; set; }
}
