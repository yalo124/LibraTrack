using LibraTrack.Data;
using LibraTrack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraTrack.Controllers;

// Same role rule as ManageCatalog: Admin and Librarian only
[Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Librarian}")]
public class ManageCopiesController : Controller
{
    private readonly ApplicationDbContext _db;
    public ManageCopiesController(ApplicationDbContext db) => _db = db;

    // GET /ManageCopies?bookId=3 : the book, its active copies, and the "add copy" form
    public async Task<IActionResult> Index(int bookId)
    {
        var book = await _db.Books.AsNoTracking()
            .Include(b => b.Copies.Where(c => !c.IsDeleted))
            .FirstOrDefaultAsync(b => b.BookId == bookId && !b.IsDeleted);
        return book == null ? NotFound() : View(book);
    }

    // POST: add ONE copy to the book
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(int bookId, string? barcode, string? branch)
    {
        barcode = (barcode ?? "").Trim();
        branch = string.IsNullOrWhiteSpace(branch) ? "Main" : branch.Trim();

        if (!await _db.Books.AnyAsync(b => b.BookId == bookId && !b.IsDeleted))
            return NotFound();

        // Manual validation (matches the lengths in BookCopy)
        string? error = null;
        if (barcode.Length == 0 || barcode.Length > 50) error = "Barcode is required (max 50 characters).";
        else if (branch.Length > 150) error = "Branch is too long (max 150 characters).";
        // Barcodes identify one physical copy, so they must be unique (needed later for 6.2)
        else if (await _db.BookCopies.AnyAsync(c => c.Barcode == barcode && !c.IsDeleted))
            error = $"Barcode \"{barcode}\" is already used.";

        if (error != null)
        {
            TempData["Error"] = error;
            return RedirectToAction(nameof(Index), new { bookId });
        }

        _db.BookCopies.Add(new BookCopy
        {
            BookId = bookId,                  // foreign key to the book
            Barcode = barcode,
            Branch = branch,
            Status = CopyStatus.Available     // every new copy starts as Available
        });
        await _db.SaveChangesAsync();         // INSERT

        TempData["Success"] = $"Copy {barcode} added.";
        return RedirectToAction(nameof(Index), new { bookId });
    }

    // POST: soft-remove one copy (lost or damaged)
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(int copyId)
    {
        var copy = await _db.BookCopies.FirstOrDefaultAsync(c => c.CopyId == copyId && !c.IsDeleted);
        if (copy == null) return NotFound();

        // A borrowed or reserved copy is in use, so it can't be removed
        if (copy.Status != CopyStatus.Available)
        {
            TempData["Error"] = "Only an Available copy can be removed.";
            return RedirectToAction(nameof(Index), new { bookId = copy.BookId });
        }

        copy.IsDeleted = true;                // soft delete, same idea as Books
        await _db.SaveChangesAsync();
        TempData["Success"] = $"Copy {copy.Barcode} removed.";
        return RedirectToAction(nameof(Index), new { bookId = copy.BookId });
    }
}
