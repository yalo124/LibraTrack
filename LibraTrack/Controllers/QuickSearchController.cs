using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using LibraTrack.Data;
using LibraTrack.Models;
using Humanizer;

namespace LibraTrack.Controllers;

[Authorize(Roles = AppRoles.Admin + "," + AppRoles.Librarian)]
public class QuickSearchController : Controller
{
    private readonly ApplicationDbContext _context;
    public QuickSearchController(ApplicationDbContext context) => _context = context;

    //GET quick search
    public async Task<IActionResult> Index(string? q)
    {
        var vm = new QuickSearchViewModel { Query = q };

        if (string.IsNullOrWhiteSpace(q))
        {
            return View(vm);
        }

        q = q.Trim();

        // find ids of the borrower roles in AspNetRoles
        var borrowerRoleIds = await _context.Roles.Where(b => b.Name == AppRoles.Student || b.Name == AppRoles.StaffFaculty).Select(b => b.Id).ToListAsync();

        //users who have one of those roles and match the text
        vm.Students = await _context.Users.Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && borrowerRoleIds.Contains(ur.RoleId)) && (u.FullName.Contains(q) || u.SchoolId!.Contains(q) || u.Email!.Contains(q)))
            .OrderBy(u => u.FullName).Take(20)
            .Select(u => new StudentResult { Id = u.Id, FullName = u.FullName, SchoolId = u.SchoolId, Email = u.Email }).ToListAsync();

        //active books matching title or isbn, with a count of available copies
        vm.Books = await _context.Books.Where(bk => !bk.IsDeleted && (bk.Title.Contains(q) || bk.ISBN.Contains(q))).OrderBy(bk => bk.Title).Take(20)
            .Select(bk => new BookResult
            {
                BookId = bk.BookId,
                Title = bk.Title,
                Author = bk.Author,
                ISBN = bk.ISBN,
                AvailableCopies = bk.Copies.Count(c => !c.IsDeleted && c.Status == CopyStatus.Available)
            }).ToListAsync();

        return View(vm);
    }

}
