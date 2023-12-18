using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP_PROJECT.Data.Migrations
{
    public partial class New : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Bookmarks_BookmarkId",
                table: "Comments");

            migrationBuilder.AlterColumn<int>(
                name: "BookmarkId",
                table: "Comments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Bookmarks_BookmarkId",
                table: "Comments",
                column: "BookmarkId",
                principalTable: "Bookmarks",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Bookmarks_BookmarkId",
                table: "Comments");

            migrationBuilder.AlterColumn<int>(
                name: "BookmarkId",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Bookmarks_BookmarkId",
                table: "Comments",
                column: "BookmarkId",
                principalTable: "Bookmarks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
