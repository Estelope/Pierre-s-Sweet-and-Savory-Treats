using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bakery.Migrations
{
    public partial class AuthDisplayName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "ApplicationUser",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "ApplicationUser");
        }
    }
}
