using System;
using Microsoft.EntityFrameworkCore.Migrations;
using PartyPlanner.Infrastructure.Security;

#nullable disable

namespace PartyPlanner.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthentication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var demoUserId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
            var passwordHasher = new Pbkdf2PasswordHasher();

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerUserId",
                table: "Parties",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: demoUserId);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    IsEmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Name", "Email", "PasswordHash", "IsEmailConfirmed", "CreatedAtUtc" },
                values: new object[]
                {
                    demoUserId,
                    "Demo Party Planner",
                    "demo@partyplanner.app",
                    passwordHasher.Hash("Party123!"),
                    true,
                    new DateTime(2026, 3, 29, 0, 0, 0, DateTimeKind.Utc)
                });

            migrationBuilder.Sql(
                "UPDATE [Parties] SET [OwnerUserId] = 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa' WHERE [OwnerUserId] = '00000000-0000-0000-0000-000000000000';");

            migrationBuilder.CreateTable(
                name: "UserExternalLogins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    ProviderUserId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    LinkedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserExternalLogins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserExternalLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Parties_OwnerUserId",
                table: "Parties",
                column: "OwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExternalLogins_Provider_ProviderUserId",
                table: "UserExternalLogins",
                columns: new[] { "Provider", "ProviderUserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserExternalLogins_UserId",
                table: "UserExternalLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Parties_Users_OwnerUserId",
                table: "Parties",
                column: "OwnerUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parties_Users_OwnerUserId",
                table: "Parties");

            migrationBuilder.DropTable(
                name: "UserExternalLogins");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Parties_OwnerUserId",
                table: "Parties");

            migrationBuilder.DropColumn(
                name: "OwnerUserId",
                table: "Parties");
        }
    }
}
