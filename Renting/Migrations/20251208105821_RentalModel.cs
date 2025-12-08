using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Renting.Migrations
{
    /// <inheritdoc />
    public partial class RentalModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CheckedOutAt",
                table: "Rentals",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CheckedOutById",
                table: "Rentals",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_CheckedOutById",
                table: "Rentals",
                column: "CheckedOutById");

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_AspNetUsers_CheckedOutById",
                table: "Rentals",
                column: "CheckedOutById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_AspNetUsers_CheckedOutById",
                table: "Rentals");

            migrationBuilder.DropIndex(
                name: "IX_Rentals_CheckedOutById",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "CheckedOutAt",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "CheckedOutById",
                table: "Rentals");
        }
    }
}
