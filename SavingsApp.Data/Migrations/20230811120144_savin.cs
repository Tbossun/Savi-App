using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SavingsApp.Data.Migrations
{
    public partial class savin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SavingsId",
                table: "personalSavingsFundings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SavingsId",
                table: "personalSavingsFundings",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
