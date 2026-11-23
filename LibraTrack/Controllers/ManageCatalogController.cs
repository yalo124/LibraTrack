using LibraTrack.Data;
using LibraTrack.Models;
using LibraTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LibraTrack.Controllers;

// Role restriction: only Admin and Librarian can reach ANY action below.
// Not logged in -> redirected to Login. Logged in but wrong role -> Access Denied page.
[Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Librarian}")]
public class ManageCatalogController : Controller
{
    private readonly ApplicationDbContext _db;
    public ManageCatalogController(ApplicationDbContext db) => _db = db;

    // GET /ManageCatalog : staff list of all non-deleted books
    public async Task<IActionResult> Index()
    {
        var books = await _db.Books.AsNoTracking()
            .Where(b => !b.IsDeleted)
            .Include(b => b.Category)
            .OrderBy(b => b.Title)
            .ToListAsync();
        return View(books);
    }

    // ---------- ADD ----------
    public async Task<IActionResult> Create()   // GET: empty form
    {
        var vm = new BookFormViewModel();
        await LoadCategoriesAsync(vm);
        return View("Form", vm);                // add and edit share one view
    }

    [HttpPost, ValidateAntiForgeryToken]        // POST: form submitted
    public async Task<IActionResult> Create(BookFormViewModel vm)
    {
        await RunExtraChecksAsync(vm);
        if (!ModelState.IsValid)                // attribute rules + ISBN rule + extra checks
        {
            await LoadCategoriesAsync(vm);      // dropdown must be rebuilt
            return View("Form", vm);            // redisplay with error messages
        }

        var book = new Book();
        CopyToBook(vm, book);
        _db.Books.Add(book);                    // INSERT
        await _db.SaveChangesAsync();

        TempData["Success"] = $"\"{book.Title}\" was added.";
        return RedirectToAction("Index", "ManageCopies", new { bookId = book.BookId });
    }

    // ---------- EDIT ----------
    public async Task<IActionResult> Edit(int? id)   // GET: form filled with current values
    {
        if (id == null) return NotFound();
        var b = await _db.Books.AsNoTracking()
            .FirstOrDefaultAsync(x => x.BookId == id && !x.IsDeleted);
        if (b == null) return NotFound();

        var vm = new BookFormViewModel
        {
            BookId = b.BookId,
            Title = b.Title,
            Author = b.Author,
            ISBN = b.ISBN,
            Publisher = b.Publisher,
            PublicationYear = b.PublicationYear,
            Description = b.Description,
            CoverImageUrl = b.CoverImageUrl,
            IsFeatured = b.IsFeatured,
            CategoryId = b.CategoryId
        };
        await LoadCategoriesAsync(vm);
        return View("Form", vm);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, BookFormViewModel vm)
    {
        if (id != vm.BookId) return BadRequest();   // URL id must match form id

        await RunExtraChecksAsync(vm);
        if (!ModelState.IsValid)
        {
            await LoadCategoriesAsync(vm);
            return View("Form", vm);
        }

        var book = await _db.Books.FirstOrDefaultAsync(x => x.BookId == id && !x.IsDeleted);
        if (book == null) return NotFound();

        CopyToBook(vm, book);                       // tracked entity -> UPDATE
        await _db.SaveChangesAsync();

        TempData["Success"] = $"\"{book.Title}\" was updated.";
        return RedirectToAction(nameof(Index));
    }

    // ---------- SOFT DELETE ----------
    public async Task<IActionResult> Delete(int? id)   // GET: "Are you sure?" page
    {
        if (id == null) return NotFound();
        var book = await _db.Books.AsNoTracking().Include(b => b.Category)
            .FirstOrDefaultAsync(b => b.BookId == id && !b.IsDeleted);
        return book == null ? NotFound() : View(book);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var book = await _db.Books.Include(b => b.Copies)
            .FirstOrDefaultAsync(b => b.BookId == id && !b.IsDeleted);
        if (book == null) return NotFound();

        // Business rule: don't hide a book while a copy is out or reserved
        if (book.Copies.Any(c => !c.IsDeleted && c.Status != CopyStatus.Available))
        {
            TempData["Error"] = "Cannot delete: a copy is currently borrowed or reserved.";
            return RedirectToAction(nameof(Index));
        }

        book.IsDeleted = true;                          // soft delete: row stays in the table
        foreach (var c in book.Copies) c.IsDeleted = true;
        await _db.SaveChangesAsync();                   // UPDATE, not DELETE

        TempData["Success"] = $"\"{book.Title}\" was deleted.";
        return RedirectToAction(nameof(Index));
    }

    // ---------- helpers ----------
    private async Task LoadCategoriesAsync(BookFormViewModel vm) =>
        vm.Categories = await _db.Categories.AsNoTracking().OrderBy(c => c.Name)
            .Select(c => new SelectListItem(c.Name, c.CategoryId.ToString()))
            .ToListAsync();

    // Checks that need the database, so they can't be done with attributes
    private async Task RunExtraChecksAsync(BookFormViewModel vm)
    {
        if (!string.IsNullOrWhiteSpace(vm.ISBN))
        {
            var isbn = BookFormViewModel.NormalizeIsbn(vm.ISBN);
            // Compare without hyphens, because the seed data stores some ISBNs with hyphens
            bool exists = await _db.Books.AnyAsync(b => !b.IsDeleted && b.BookId != vm.BookId
                && b.ISBN.Replace("-", "").Replace(" ", "") == isbn);
            if (exists) ModelState.AddModelError(nameof(vm.ISBN), "A book with this ISBN already exists.");
        }
        if (!await _db.Categories.AnyAsync(c => c.CategoryId == vm.CategoryId))
            ModelState.AddModelError(nameof(vm.CategoryId), "Please choose a valid category.");
    }

    // Copies ONLY the allowed fields. IsDeleted is never touched here.
    private static void CopyToBook(BookFormViewModel vm, Book book)
    {
        book.Title = vm.Title.Trim();
        book.Author = vm.Author.Trim();
        book.ISBN = BookFormViewModel.NormalizeIsbn(vm.ISBN);   // stored without hyphens
        book.Publisher = vm.Publisher;
        book.PublicationYear = vm.PublicationYear;
        book.Description = vm.Description;
        book.CoverImageUrl = vm.CoverImageUrl;
        book.IsFeatured = vm.IsFeatured;
        book.CategoryId = vm.CategoryId;
    }
}
