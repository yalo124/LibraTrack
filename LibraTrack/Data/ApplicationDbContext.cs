using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LibraTrack.Models;

namespace LibraTrack.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<BookCopy> BookCopies { get; set; }
    public DbSet<Borrowing> Borrowings { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Category>().HasData(
            new Category { CategoryId = 1, Name = "Programming", Description = "Software and coding books" },
            new Category { CategoryId = 2, Name = "Programming", Description = "Software and coding books" }
        );

        builder.Entity<Book>().HasData(
            new Book
            {
                BookId = 1,
                Title = "Clean Code",
                Author = "Robert C. Martin",
                ISBN = "9780132350884",
                CategoryId = 1,
                IsFeatured = true
            },
            new Book
            {
                BookId = 2,
                Title = "Pyhton Pocket",
                Author = "Mark Lutz",
                ISBN = "978-0-596-15808-8",
                CategoryId = 2,
                IsFeatured = true
            }
        );

        builder.Entity<BookCopy>().HasData(
            new BookCopy { CopyId = 1, BookId = 1, Barcode = "LIB-0001", Branch = "Main", Status = CopyStatus.Available },
            new BookCopy { CopyId = 2, BookId = 1, Barcode = "LIB-0002", Branch = "Main", Status = CopyStatus.Available },
            new BookCopy { CopyId = 3, BookId = 2, Barcode = "LIB-0003", Branch = "Main", Status = CopyStatus.Available }

        );

        builder.Entity<Book>().HasIndex(b => b.Title);
        builder.Entity<Book>().HasIndex(b => b.Author);
        builder.Entity<Book>().HasIndex(b => b.ISBN);
    }
}
