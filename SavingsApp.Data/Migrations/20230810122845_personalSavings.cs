using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SavingsApp.Data.Migrations
{
    public partial class personalSavings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SavingId",
                table: "personalSavingsFundings",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SavingId",
                table: "personalSavingsFundings");
        }
    }
}
