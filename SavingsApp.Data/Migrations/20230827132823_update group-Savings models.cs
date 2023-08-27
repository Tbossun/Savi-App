using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SavingsApp.Data.Migrations
{
    public partial class updategroupSavingsmodels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupSavingsFundings_AspNetUsers_UserId",
                table: "GroupSavingsFundings");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupSavingsMembers_AspNetUsers_ApplicationUserId",
                table: "GroupSavingsMembers");

            migrationBuilder.DropIndex(
                name: "IX_GroupSavingsMembers_ApplicationUserId",
                table: "GroupSavingsMembers");

            migrationBuilder.DropIndex(
                name: "IX_GroupSavingsFundings_UserId",
                table: "GroupSavingsFundings");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "GroupSavingsMembers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "GroupSavingsFundings");

            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "GroupSavingsMembers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "GroupSavingsMembers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_GroupSavingsMembers_UserId",
                table: "GroupSavingsMembers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupSavingsMembers_AspNetUsers_UserId",
                table: "GroupSavingsMembers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupSavingsMembers_AspNetUsers_UserId",
                table: "GroupSavingsMembers");

            migrationBuilder.DropIndex(
                name: "IX_GroupSavingsMembers_UserId",
                table: "GroupSavingsMembers");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "GroupSavingsMembers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "GroupSavingsMembers");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "GroupSavingsMembers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "GroupSavingsFundings",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_GroupSavingsMembers_ApplicationUserId",
                table: "GroupSavingsMembers",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupSavingsFundings_UserId",
                table: "GroupSavingsFundings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupSavingsFundings_AspNetUsers_UserId",
                table: "GroupSavingsFundings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupSavingsMembers_AspNetUsers_ApplicationUserId",
                table: "GroupSavingsMembers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
