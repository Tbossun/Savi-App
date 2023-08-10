using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SavingsApp.Data.Migrations
{
    public partial class paymentSavings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WalletFundings_Wallets_walletId1",
                table: "WalletFundings");

            migrationBuilder.DropIndex(
                name: "IX_WalletFundings_walletId1",
                table: "WalletFundings");

            migrationBuilder.DropColumn(
                name: "walletId1",
                table: "WalletFundings");

            migrationBuilder.AlterColumn<string>(
                name: "walletId",
                table: "WalletFundings",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    CategoryName = table.Column<string>(type: "TEXT", nullable: false),
                    CategoryDescription = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "frequencies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    FrequencyId = table.Column<string>(type: "TEXT", nullable: false),
                    FrequencyName = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_frequencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "personalSavingsFundings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    ActionType = table.Column<int>(type: "INTEGER", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    CumulativeAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personalSavingsFundings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "personalSavings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    SafeCategoryId = table.Column<string>(type: "TEXT", nullable: false),
                    SaveName = table.Column<string>(type: "TEXT", nullable: false),
                    TargetAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    TargetDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CurrentAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    AutoSave = table.Column<bool>(type: "INTEGER", nullable: false),
                    AutoSaveAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    FrequencyId = table.Column<string>(type: "TEXT", nullable: false),
                    MaxLimit = table.Column<decimal>(type: "TEXT", nullable: false),
                    SavingsImageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    categoryId = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personalSavings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_personalSavings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_personalSavings_categories_categoryId",
                        column: x => x.categoryId,
                        principalTable: "categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WalletFundings_walletId",
                table: "WalletFundings",
                column: "walletId");

            migrationBuilder.CreateIndex(
                name: "IX_personalSavings_categoryId",
                table: "personalSavings",
                column: "categoryId");

            migrationBuilder.CreateIndex(
                name: "IX_personalSavings_UserId",
                table: "personalSavings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WalletFundings_Wallets_walletId",
                table: "WalletFundings",
                column: "walletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WalletFundings_Wallets_walletId",
                table: "WalletFundings");

            migrationBuilder.DropTable(
                name: "frequencies");

            migrationBuilder.DropTable(
                name: "personalSavings");

            migrationBuilder.DropTable(
                name: "personalSavingsFundings");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropIndex(
                name: "IX_WalletFundings_walletId",
                table: "WalletFundings");

            migrationBuilder.AlterColumn<int>(
                name: "walletId",
                table: "WalletFundings",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "walletId1",
                table: "WalletFundings",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WalletFundings_walletId1",
                table: "WalletFundings",
                column: "walletId1");

            migrationBuilder.AddForeignKey(
                name: "FK_WalletFundings_Wallets_walletId1",
                table: "WalletFundings",
                column: "walletId1",
                principalTable: "Wallets",
                principalColumn: "Id");
        }
    }
}
