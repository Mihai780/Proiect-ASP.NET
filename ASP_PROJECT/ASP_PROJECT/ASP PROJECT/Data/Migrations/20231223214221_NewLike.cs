using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP_PROJECT.Data.Migrations
{
    public partial class NewLike : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "Bookmarks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Likes",
                table: "Bookmarks");
        }
    }
}
