using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SavingsApp.Data.Migrations
{
    public partial class savings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SavingId",
                table: "personalSavingsFundings",
                newName: "SavingsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SavingsId",
                table: "personalSavingsFundings",
                newName: "SavingId");
        }
    }
}
