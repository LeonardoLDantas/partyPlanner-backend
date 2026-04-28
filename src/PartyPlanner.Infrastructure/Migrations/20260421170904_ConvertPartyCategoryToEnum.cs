using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PartyPlanner.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConvertPartyCategoryToEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryTemp",
                table: "Parties",
                type: "integer",
                nullable: false,
                defaultValue: 6);

            migrationBuilder.Sql("""
                UPDATE "Parties"
                SET "CategoryTemp" =
                    CASE
                        WHEN "Category" IN ('Aniversario', 'Aniversário') THEN 1
                        WHEN "Category" = 'Festa' THEN 2
                        WHEN "Category" = 'Formatura' THEN 3
                        WHEN "Category" = 'Casamento' THEN 4
                        WHEN "Category" = 'Noivado' THEN 5
                        ELSE 6
                    END;
                """);

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Parties");

            migrationBuilder.RenameColumn(
                name: "CategoryTemp",
                table: "Parties",
                newName: "Category");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CategoryTemp",
                table: "Parties",
                type: "character varying(80)",
                maxLength: 80,
                nullable: false,
                defaultValue: "Outros");

            migrationBuilder.Sql("""
                UPDATE "Parties"
                SET "CategoryTemp" =
                    CASE
                        WHEN "Category" = 1 THEN 'Aniversario'
                        WHEN "Category" = 2 THEN 'Festa'
                        WHEN "Category" = 3 THEN 'Formatura'
                        WHEN "Category" = 4 THEN 'Casamento'
                        WHEN "Category" = 5 THEN 'Noivado'
                        ELSE 'Outros'
                    END;
                """);

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Parties");

            migrationBuilder.RenameColumn(
                name: "CategoryTemp",
                table: "Parties",
                newName: "Category");
        }
    }
}
