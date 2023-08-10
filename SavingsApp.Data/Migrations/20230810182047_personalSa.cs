using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SavingsApp.Data.Migrations
{
    public partial class personalSa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_personalSavings_categories_categoryId",
                table: "personalSavings");

            migrationBuilder.RenameColumn(
                name: "categoryId",
                table: "personalSavings",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_personalSavings_categoryId",
                table: "personalSavings",
                newName: "IX_personalSavings_CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_personalSavings_categories_CategoryId",
                table: "personalSavings",
                column: "CategoryId",
                principalTable: "categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_personalSavings_categories_CategoryId",
                table: "personalSavings");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "personalSavings",
                newName: "categoryId");

            migrationBuilder.RenameIndex(
                name: "IX_personalSavings_CategoryId",
                table: "personalSavings",
                newName: "IX_personalSavings_categoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_personalSavings_categories_categoryId",
                table: "personalSavings",
                column: "categoryId",
                principalTable: "categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
