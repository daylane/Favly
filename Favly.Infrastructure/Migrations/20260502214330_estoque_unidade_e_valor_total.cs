using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Favly.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class estoque_unidade_e_valor_total : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Preco",
                table: "Movimentacoes",
                newName: "ValorTotal");

            // Ajusta precisão: Preco era (18,4), ValorTotal deve ser (18,2)
            migrationBuilder.AlterColumn<decimal>(
                name: "ValorTotal",
                table: "Movimentacoes",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,4)",
                oldPrecision: 18,
                oldScale: 4,
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PrecoUnitario",
                table: "Movimentacoes",
                type: "numeric(18,4)",
                precision: 18,
                scale: 4,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrecoUnitario",
                table: "Movimentacoes");

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorTotal",
                table: "Movimentacoes",
                type: "numeric(18,4)",
                precision: 18,
                scale: 4,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.RenameColumn(
                name: "ValorTotal",
                table: "Movimentacoes",
                newName: "Preco");
        }
    }
}
