using LibraTrack.Data;
using LibraTrack.Models;
using LibraTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraTrack.Controllers;

[AllowAnonymous]   // Guests allowed: no login or role needed for browsing
public class CatalogController : Controller
{
    private readonly ApplicationDbContext _db;
    private const int PageSize = 1;   // books per page, temporarily set to 1 for testing due to low current low number of books in the DB

    public CatalogController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index(int page = 1)
    {
        if (page < 1) page = 1;

        var books = _db.Books.AsNoTracking().Where(b => !b.IsDeleted);

        var total = await books.CountAsync();

        var items = await books
            .Include(b => b.Category)
            .Include(b => b.Copies.Where(c => !c.IsDeleted))
            .OrderBy(b => b.Title)
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();

        return View(new PagedResult<Book>
        {
            Items = items,
            Page = page,
            PageSize = PageSize,
            TotalItems = total
        });
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var book = await _db.Books
            .AsNoTracking()
            .Include(b => b.Category)
            .Include(b => b.Copies.Where(c => !c.IsDeleted))
            .FirstOrDefaultAsync(b => b.BookId == id && !b.IsDeleted);

        if (book == null) return NotFound();

        return View(book);
    }
}