using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PartyPlanner.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NormalizeBudgetItemCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                UPDATE "BudgetItems"
                SET "Category" = CASE
                    WHEN "Category" IN ('Alimentacao', 'Decoracao', 'Local', 'Musica', 'FotoVideo', 'Lembrancas', 'Transporte', 'Equipe', 'Outros') THEN "Category"
                    WHEN "Category" IN ('Ambiente', 'Decoracao e ambiente', 'Decoração', 'Decoracao') THEN 'Decoracao'
                    WHEN "Category" IN ('Buffet', 'Comida', 'Bebida', 'Alimentos') THEN 'Alimentacao'
                    ELSE 'Outros'
                END
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
