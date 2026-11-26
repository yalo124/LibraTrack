using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using LibraTrack.Data;
using LibraTrack.Models;

namespace LibraTrack.Controllers;

[AllowAnonymous] //Guest included
public class SearchController : Controller
{
    private const int PageSize = 10;
    private readonly ApplicationDbContext _context;

    public SearchController(ApplicationDbContext context) => _context = context;

    //GET
    public async Task<IActionResult> Index(string? keyword, int? categoryId, int page = 1)
    {
        var query = _context.Books.Include(b => b.Category).Where(b => !b.IsDeleted);

        if(!string.IsNullOrWhiteSpace(keyword))
        {
            keyword = keyword.Trim();
            query = query.Where(b => b.Title.Contains(keyword) || b.Author.Contains(keyword) || b.ISBN.Contains(keyword));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(b => b.CategoryId == categoryId.Value);
        }

        // Paging: count first, then fetch only one page
        int total = await query.CountAsync();
        int totalPages = (int)Math.Ceiling(total / (double)PageSize);
        page = Math.Max(1, Math.Min(page, Math.Max(1, totalPages))); // Keep the page number within range

        var books = await query.OrderBy(b => b.Title).Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();

        var vm = new SearchResultViewModel
        {
            Books = books,
            Categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync(),
            Keyword = keyword,
            CategoryId = categoryId,
            Page = page,
            TotalPages = totalPages,
            TotalResults = total
        };

        return Json(vm.Books.Select(b => b.Title));
    }

}

