using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP_PROJECT.Data.Migrations
{
    public partial class Edi_DB_categ_manytomany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookmarkCategory_Bookmarks_BookmarkId",
                table: "BookmarkCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_BookmarkCategory_Category_CategoryId",
                table: "BookmarkCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookmarkCategory",
                table: "BookmarkCategory");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "Categories");

            migrationBuilder.RenameTable(
                name: "BookmarkCategory",
                newName: "BookmarkCategories");

            migrationBuilder.RenameIndex(
                name: "IX_BookmarkCategory_CategoryId",
                table: "BookmarkCategories",
                newName: "IX_BookmarkCategories_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_BookmarkCategory_BookmarkId",
                table: "BookmarkCategories",
                newName: "IX_BookmarkCategories_BookmarkId");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "BookmarkCategories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BookmarkId",
                table: "BookmarkCategories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookmarkCategories",
                table: "BookmarkCategories",
                columns: new[] { "Id", "BookmarkId", "CategoryId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BookmarkCategories_Bookmarks_BookmarkId",
                table: "BookmarkCategories",
                column: "BookmarkId",
                principalTable: "Bookmarks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookmarkCategories_Categories_CategoryId",
                table: "BookmarkCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookmarkCategories_Bookmarks_BookmarkId",
                table: "BookmarkCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_BookmarkCategories_Categories_CategoryId",
                table: "BookmarkCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookmarkCategories",
                table: "BookmarkCategories");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Category");

            migrationBuilder.RenameTable(
                name: "BookmarkCategories",
                newName: "BookmarkCategory");

            migrationBuilder.RenameIndex(
                name: "IX_BookmarkCategories_CategoryId",
                table: "BookmarkCategory",
                newName: "IX_BookmarkCategory_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_BookmarkCategories_BookmarkId",
                table: "BookmarkCategory",
                newName: "IX_BookmarkCategory_BookmarkId");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "BookmarkCategory",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BookmarkId",
                table: "BookmarkCategory",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookmarkCategory",
                table: "BookmarkCategory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookmarkCategory_Bookmarks_BookmarkId",
                table: "BookmarkCategory",
                column: "BookmarkId",
                principalTable: "Bookmarks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookmarkCategory_Category_CategoryId",
                table: "BookmarkCategory",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id");
        }
    }
}
