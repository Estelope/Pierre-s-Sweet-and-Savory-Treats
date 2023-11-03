using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bakery.Migrations
{
    public partial class authflavor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Flavors",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Flavors_UserId",
                table: "Flavors",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flavors_ApplicationUser_UserId",
                table: "Flavors",
                column: "UserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flavors_ApplicationUser_UserId",
                table: "Flavors");

            migrationBuilder.DropIndex(
                name: "IX_Flavors_UserId",
                table: "Flavors");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Flavors");
        }
    }
}
