using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SavingsApp.Data.Migrations
{
    public partial class groupSavingsmodels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupSavingsFunding_AspNetUsers_UserId",
                table: "GroupSavingsFunding");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupSavingsFunding_GroupSavings_GroupSavingId",
                table: "GroupSavingsFunding");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupSavingsMember_AspNetUsers_ApplicationUserId",
                table: "GroupSavingsMember");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupSavingsMember_GroupSavings_GroupSavingId",
                table: "GroupSavingsMember");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupSavingsMember",
                table: "GroupSavingsMember");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupSavingsFunding",
                table: "GroupSavingsFunding");

            migrationBuilder.RenameTable(
                name: "GroupSavingsMember",
                newName: "GroupSavingsMembers");

            migrationBuilder.RenameTable(
                name: "GroupSavingsFunding",
                newName: "GroupSavingsFundings");

            migrationBuilder.RenameIndex(
                name: "IX_GroupSavingsMember_GroupSavingId",
                table: "GroupSavingsMembers",
                newName: "IX_GroupSavingsMembers_GroupSavingId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupSavingsMember_ApplicationUserId",
                table: "GroupSavingsMembers",
                newName: "IX_GroupSavingsMembers_ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupSavingsFunding_UserId",
                table: "GroupSavingsFundings",
                newName: "IX_GroupSavingsFundings_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupSavingsFunding_GroupSavingId",
                table: "GroupSavingsFundings",
                newName: "IX_GroupSavingsFundings_GroupSavingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupSavingsMembers",
                table: "GroupSavingsMembers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupSavingsFundings",
                table: "GroupSavingsFundings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupSavingsFundings_AspNetUsers_UserId",
                table: "GroupSavingsFundings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupSavingsFundings_GroupSavings_GroupSavingId",
                table: "GroupSavingsFundings",
                column: "GroupSavingId",
                principalTable: "GroupSavings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupSavingsMembers_AspNetUsers_ApplicationUserId",
                table: "GroupSavingsMembers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupSavingsMembers_GroupSavings_GroupSavingId",
                table: "GroupSavingsMembers",
                column: "GroupSavingId",
                principalTable: "GroupSavings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupSavingsFundings_AspNetUsers_UserId",
                table: "GroupSavingsFundings");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupSavingsFundings_GroupSavings_GroupSavingId",
                table: "GroupSavingsFundings");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupSavingsMembers_AspNetUsers_ApplicationUserId",
                table: "GroupSavingsMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupSavingsMembers_GroupSavings_GroupSavingId",
                table: "GroupSavingsMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupSavingsMembers",
                table: "GroupSavingsMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupSavingsFundings",
                table: "GroupSavingsFundings");

            migrationBuilder.RenameTable(
                name: "GroupSavingsMembers",
                newName: "GroupSavingsMember");

            migrationBuilder.RenameTable(
                name: "GroupSavingsFundings",
                newName: "GroupSavingsFunding");

            migrationBuilder.RenameIndex(
                name: "IX_GroupSavingsMembers_GroupSavingId",
                table: "GroupSavingsMember",
                newName: "IX_GroupSavingsMember_GroupSavingId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupSavingsMembers_ApplicationUserId",
                table: "GroupSavingsMember",
                newName: "IX_GroupSavingsMember_ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupSavingsFundings_UserId",
                table: "GroupSavingsFunding",
                newName: "IX_GroupSavingsFunding_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupSavingsFundings_GroupSavingId",
                table: "GroupSavingsFunding",
                newName: "IX_GroupSavingsFunding_GroupSavingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupSavingsMember",
                table: "GroupSavingsMember",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupSavingsFunding",
                table: "GroupSavingsFunding",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupSavingsFunding_AspNetUsers_UserId",
                table: "GroupSavingsFunding",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupSavingsFunding_GroupSavings_GroupSavingId",
                table: "GroupSavingsFunding",
                column: "GroupSavingId",
                principalTable: "GroupSavings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupSavingsMember_AspNetUsers_ApplicationUserId",
                table: "GroupSavingsMember",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupSavingsMember_GroupSavings_GroupSavingId",
                table: "GroupSavingsMember",
                column: "GroupSavingId",
                principalTable: "GroupSavings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
