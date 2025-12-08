using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Renting.Migrations
{
    /// <inheritdoc />
    public partial class idk5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Condition",
                table: "Rentals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DamageNotes",
                table: "Rentals",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Condition",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "DamageNotes",
                table: "Rentals");
        }
    }
}
