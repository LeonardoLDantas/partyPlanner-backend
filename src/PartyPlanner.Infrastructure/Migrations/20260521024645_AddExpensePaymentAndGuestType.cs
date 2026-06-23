using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PartyPlanner.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExpensePaymentAndGuestType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Guests",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "Adulto");

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "BudgetItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Guests");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "BudgetItems");
        }
    }
}
