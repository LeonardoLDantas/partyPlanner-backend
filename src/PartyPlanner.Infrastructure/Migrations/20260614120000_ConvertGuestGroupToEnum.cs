using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PartyPlanner.Infrastructure.Migrations;

/// <inheritdoc />
public partial class ConvertGuestGroupToEnum : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Add new int column with default (6 = Outros)
        migrationBuilder.AddColumn<int>(
            name: "GroupEnum",
            table: "Guests",
            type: "integer",
            nullable: false,
            defaultValue: 6);

        // Map known string values to enum ints; everything else → Outros (6)
        migrationBuilder.Sql(@"
            UPDATE ""Guests"" SET ""GroupEnum"" = CASE
                WHEN ""Group"" ILIKE 'família'   OR ""Group"" ILIKE 'familia'   THEN 1
                WHEN ""Group"" ILIKE 'amigos'    OR ""Group"" ILIKE 'amigo'     THEN 2
                WHEN ""Group"" ILIKE 'trabalho'                                 THEN 3
                WHEN ""Group"" ILIKE 'escola'    OR ""Group"" ILIKE 'faculdade' THEN 4
                WHEN ""Group"" ILIKE 'vizinhos'  OR ""Group"" ILIKE 'vizinho'   THEN 5
                ELSE 6
            END;
        ");

        // Drop old string column and rename
        migrationBuilder.DropColumn(name: "Group", table: "Guests");
        migrationBuilder.RenameColumn(name: "GroupEnum", newName: "Group", table: "Guests");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "GroupOld",
            table: "Guests",
            type: "character varying(100)",
            maxLength: 100,
            nullable: false,
            defaultValue: "Outros");

        migrationBuilder.Sql(@"
            UPDATE ""Guests"" SET ""GroupOld"" = CASE ""Group""
                WHEN 1 THEN 'Família'
                WHEN 2 THEN 'Amigos'
                WHEN 3 THEN 'Trabalho'
                WHEN 4 THEN 'Escola'
                WHEN 5 THEN 'Vizinhos'
                ELSE 'Outros'
            END;
        ");

        migrationBuilder.DropColumn(name: "Group", table: "Guests");
        migrationBuilder.RenameColumn(name: "GroupOld", newName: "Group", table: "Guests");
    }
}
