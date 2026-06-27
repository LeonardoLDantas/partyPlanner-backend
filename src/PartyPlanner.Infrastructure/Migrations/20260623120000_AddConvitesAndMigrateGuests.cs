using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PartyPlanner.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddConvitesAndMigrateGuests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Create Convites table
            migrationBuilder.CreateTable(
                name: "Convites",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PartyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Observacao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    SenhaPresente = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Convites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Convites_Parties_PartyId",
                        column: x => x.PartyId,
                        principalTable: "Parties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(name: "IX_Convites_PartyId", table: "Convites", column: "PartyId");

            // 2. Create ConviteSenhas table
            migrationBuilder.CreateTable(
                name: "ConviteSenhas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ConviteId = table.Column<Guid>(type: "uuid", nullable: false),
                    Codigo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConviteSenhas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConviteSenhas_Convites_ConviteId",
                        column: x => x.ConviteId,
                        principalTable: "Convites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(name: "IX_ConviteSenhas_ConviteId", table: "ConviteSenhas", column: "ConviteId");
            migrationBuilder.CreateIndex(name: "IX_ConviteSenhas_Codigo", table: "ConviteSenhas", column: "Codigo", unique: true);

            // 3. Add ConviteId column to Guests (nullable)
            migrationBuilder.AddColumn<Guid>(
                name: "ConviteId",
                table: "Guests",
                type: "uuid",
                nullable: true);

            // 4. Create a default Convite for each party that has guests,
            //    then assign existing guests to it
            migrationBuilder.Sql("""
                INSERT INTO "Convites" ("Id", "PartyId", "Nome", "Observacao", "Tipo", "SenhaPresente", "CreatedAt")
                SELECT
                    gen_random_uuid(),
                    DISTINCT_PARTIES."PartyId",
                    'Geral',
                    '',
                    1,
                    '',
                    NOW()
                FROM (
                    SELECT DISTINCT "PartyId" FROM "Guests" WHERE "PartyId" IS NOT NULL
                ) AS DISTINCT_PARTIES;
            """);

            migrationBuilder.Sql("""
                UPDATE "Guests" g
                SET "ConviteId" = c."Id"
                FROM "Convites" c
                WHERE c."PartyId" = g."PartyId"
                  AND g."ConviteId" IS NULL;
            """);

            // 5. Make ConviteId NOT NULL
            migrationBuilder.AlterColumn<Guid>(
                name: "ConviteId",
                table: "Guests",
                type: "uuid",
                nullable: false,
                defaultValue: Guid.Empty,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            // 6. Add FK and index for ConviteId
            migrationBuilder.CreateIndex(name: "IX_Guests_ConviteId", table: "Guests", column: "ConviteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Guests_Convites_ConviteId",
                table: "Guests",
                column: "ConviteId",
                principalTable: "Convites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            // 7. Drop old PartyId FK and column from Guests
            migrationBuilder.DropForeignKey(name: "FK_Guests_Parties_PartyId", table: "Guests");
            migrationBuilder.DropIndex(name: "IX_Guests_PartyId", table: "Guests");
            migrationBuilder.DropColumn(name: "PartyId", table: "Guests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(name: "PartyId", table: "Guests", type: "uuid", nullable: true);
            migrationBuilder.DropForeignKey(name: "FK_Guests_Convites_ConviteId", table: "Guests");
            migrationBuilder.DropIndex(name: "IX_Guests_ConviteId", table: "Guests");
            migrationBuilder.DropColumn(name: "ConviteId", table: "Guests");
            migrationBuilder.DropTable(name: "ConviteSenhas");
            migrationBuilder.DropTable(name: "Convites");
        }
    }
}
