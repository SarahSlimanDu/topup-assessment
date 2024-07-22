using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TopUpBeneficiary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTopAmountToTopUpOptionIdInTopUpTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TopUpAmount",
                table: "TopUpTransactions");

            migrationBuilder.AddColumn<Guid>(
                name: "TopUpOptionId",
                table: "TopUpTransactions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TopUpTransactions_TopUpOptionId",
                table: "TopUpTransactions",
                column: "TopUpOptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_TopUpTransactions_TopUpOptions_TopUpOptionId",
                table: "TopUpTransactions",
                column: "TopUpOptionId",
                principalTable: "TopUpOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TopUpTransactions_TopUpOptions_TopUpOptionId",
                table: "TopUpTransactions");

            migrationBuilder.DropIndex(
                name: "IX_TopUpTransactions_TopUpOptionId",
                table: "TopUpTransactions");

            migrationBuilder.DropColumn(
                name: "TopUpOptionId",
                table: "TopUpTransactions");

            migrationBuilder.AddColumn<int>(
                name: "TopUpAmount",
                table: "TopUpTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
