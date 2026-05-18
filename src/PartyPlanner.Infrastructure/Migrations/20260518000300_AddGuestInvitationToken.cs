using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PartyPlanner.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGuestInvitationToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InvitationToken",
                table: "Guests",
                type: "character varying(80)",
                maxLength: 80,
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql("""
                UPDATE "Guests"
                SET "InvitationToken" = lower(replace("Id"::text, '-', ''))
                WHERE "InvitationToken" = '';
                """);

            migrationBuilder.CreateIndex(
                name: "IX_Guests_InvitationToken",
                table: "Guests",
                column: "InvitationToken",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Guests_InvitationToken",
                table: "Guests");

            migrationBuilder.DropColumn(
                name: "InvitationToken",
                table: "Guests");
        }
    }
}
