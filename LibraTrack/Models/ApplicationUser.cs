using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LibraTrack.Models;
public class ApplicationUser : IdentityUser
{
    [Required, StringLength(100)] public string Full_Name { get; set; } = "";
    [StringLength(30)] public string? School_Id { get; set; }
}

