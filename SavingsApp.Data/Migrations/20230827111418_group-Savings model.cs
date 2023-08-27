using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SavingsApp.Data.Migrations
{
    public partial class groupSavingsmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SavingsImageUrl",
                table: "personalSavings",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.CreateTable(
                name: "GroupSavings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    SaveName = table.Column<string>(type: "TEXT", nullable: false),
                    ContributionAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    ExpectedStartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ActualStartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ExpectedEndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CurrentAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    MemberCount = table.Column<int>(type: "INTEGER", nullable: false),
                    RunTime = table.Column<int>(type: "INTEGER", nullable: false),
                    NextRunTime = table.Column<int>(type: "INTEGER", nullable: false),
                    PurposeAndGoal = table.Column<string>(type: "TEXT", nullable: false),
                    TermsAndCondition = table.Column<string>(type: "TEXT", nullable: false),
                    AutoSaveAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    FrequencyId = table.Column<string>(type: "TEXT", nullable: false),
                    GroupStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxLimit = table.Column<decimal>(type: "TEXT", nullable: false),
                    SavePortraitImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    SetLandScapeImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupSavings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupSavings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupSavingsFunding",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    GroupSavingsId = table.Column<string>(type: "TEXT", nullable: false),
                    ActionType = table.Column<int>(type: "INTEGER", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    GroupSavingId = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupSavingsFunding", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupSavingsFunding_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupSavingsFunding_GroupSavings_GroupSavingId",
                        column: x => x.GroupSavingId,
                        principalTable: "GroupSavings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GroupSavingsMember",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    IsGroupOwner = table.Column<bool>(type: "INTEGER", nullable: false),
                    GroupSavingId = table.Column<string>(type: "TEXT", nullable: false),
                    LastSavingDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ApplicationUserId = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupSavingsMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupSavingsMember_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GroupSavingsMember_GroupSavings_GroupSavingId",
                        column: x => x.GroupSavingId,
                        principalTable: "GroupSavings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupSavings_UserId",
                table: "GroupSavings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupSavingsFunding_GroupSavingId",
                table: "GroupSavingsFunding",
                column: "GroupSavingId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupSavingsFunding_UserId",
                table: "GroupSavingsFunding",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupSavingsMember_ApplicationUserId",
                table: "GroupSavingsMember",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupSavingsMember_GroupSavingId",
                table: "GroupSavingsMember",
                column: "GroupSavingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupSavingsFunding");

            migrationBuilder.DropTable(
                name: "GroupSavingsMember");

            migrationBuilder.DropTable(
                name: "GroupSavings");

            migrationBuilder.AlterColumn<string>(
                name: "SavingsImageUrl",
                table: "personalSavings",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
