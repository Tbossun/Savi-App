using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SavingsApp.Data.Migrations
{
    public partial class personalSav : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SafeCategoryId",
                table: "personalSavings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SafeCategoryId",
                table: "personalSavings",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
