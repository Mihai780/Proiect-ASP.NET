using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP_PROJECT.Data.Migrations
{
    public partial class Edi_DB_adaugare_categ : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookmarkCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookmarkId = table.Column<int>(type: "int", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    BookmarkDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookmarkCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookmarkCategory_Bookmarks_BookmarkId",
                        column: x => x.BookmarkId,
                        principalTable: "Bookmarks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BookmarkCategory_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookmarkCategory_BookmarkId",
                table: "BookmarkCategory",
                column: "BookmarkId");

            migrationBuilder.CreateIndex(
                name: "IX_BookmarkCategory_CategoryId",
                table: "BookmarkCategory",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookmarkCategory");

            migrationBuilder.DropTable(
                name: "Category");
        }
    }
}
