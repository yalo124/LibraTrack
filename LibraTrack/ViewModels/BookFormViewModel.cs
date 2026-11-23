using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraTrack.ViewModels;

// The add/edit form's data. IValidatableObject lets us add a custom rule (ISBN check).
public class BookFormViewModel : IValidatableObject
{
    public int BookId { get; set; }   // 0 = adding a new book, >0 = editing

    [Required, StringLength(250)]
    public string Title { get; set; } = "";

    [Required, StringLength(150)]
    public string Author { get; set; } = "";

    [Required, StringLength(20)]
    [Display(Name = "ISBN")]
    public string ISBN { get; set; } = "";

    [StringLength(150)]
    public string? Publisher { get; set; }

    [Range(1000, 2100, ErrorMessage = "Enter a valid year.")]
    [Display(Name = "Publication year")]
    public int? PublicationYear { get; set; }

    [StringLength(2000)]
    public string? Description { get; set; }

    [StringLength(500), Url]
    [Display(Name = "Cover image URL")]
    public string? CoverImageUrl { get; set; }

    [Display(Name = "Featured on front page")]
    public bool IsFeatured { get; set; }

    // 0 means "nothing chosen" in the dropdown, so Range(1, ...) rejects it
    [Range(1, int.MaxValue, ErrorMessage = "Please choose a category.")]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }

    // Items for the category dropdown. Filled by the controller, not typed by the user,
    // so [ValidateNever] stops it from being validated.
    [ValidateNever]
    public List<SelectListItem> Categories { get; set; } = new();

    // Custom rule: runs after the attribute rules above have passed
    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (!IsValidIsbn(ISBN))
            yield return new ValidationResult(
                "Enter a valid ISBN-10 or ISBN-13 (hyphens allowed).",
                new[] { nameof(ISBN) });   // attaches the error to the ISBN field
    }

    // "978-0-13-235088-4" -> "9780132350884" (also used by the controller before saving)
    public static string NormalizeIsbn(string isbn) =>
        isbn.Replace("-", "").Replace(" ", "").ToUpperInvariant();

    public static bool IsValidIsbn(string isbn)
    {
        var s = NormalizeIsbn(isbn);

        if (s.Length == 13 && s.All(char.IsDigit))        // ISBN-13: weights 1,3,1,3...
        {
            int sum = 0;
            for (int i = 0; i < 12; i++) sum += (s[i] - '0') * (i % 2 == 0 ? 1 : 3);
            return (10 - sum % 10) % 10 == s[12] - '0';   // last digit is the check digit
        }

        if (s.Length == 10 && s[..9].All(char.IsDigit)    // ISBN-10: weights 10..1
            && (char.IsDigit(s[9]) || s[9] == 'X'))
        {
            int sum = 0;
            for (int i = 0; i < 9; i++) sum += (s[i] - '0') * (10 - i);
            sum += s[9] == 'X' ? 10 : s[9] - '0';         // 'X' stands for 10
            return sum % 11 == 0;
        }

        return false;
    }
}
