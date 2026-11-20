namespace LibraTrack.Models;
public class App_Roles
{
    public const string Admin = "Admin";
    public const string Librarian = "Librarian";
    public const string StaffFaculty = "Staff / Faculty";
    public const string Student = "Student";

    public static readonly string[] All = { Admin, Librarian, StaffFaculty, Student };
}

