using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraTrack.Migrations
{
    /// <inheritdoc />
    public partial class AddNewBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 2,
                columns: new[] { "CategoryId", "ISBN" },
                values: new object[] { 1, "978059615808" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "BookId", "Author", "CategoryId", "CoverImageUrl", "Description", "ISBN", "IsDeleted", "IsFeatured", "PublicationYear", "Publisher", "Title" },
                values: new object[] { 101, " Tomohito Oda", 2, null, null, "978059615825", false, true, null, null, "Komi Can't Communicate" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Japanase Comic", "Manga" });

            migrationBuilder.InsertData(
                table: "BookCopies",
                columns: new[] { "CopyId", "Barcode", "BookId", "Branch", "IsDeleted", "Status" },
                values: new object[] { 101, "LIB-0004", 101, "Main", false, 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BookCopies",
                keyColumn: "CopyId",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 101);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 2,
                columns: new[] { "CategoryId", "ISBN" },
                values: new object[] { 2, "978-0-596-15808-8" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Software and coding books", "Programming" });
        }
    }
}
