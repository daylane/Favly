using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Favly.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CorrecaoTarefaPagamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Recorrencia_DiasDaSemana",
                table: "Pagamentos");

            migrationBuilder.RenameColumn(
                name: "RecorrenciaIntervalo",
                table: "Pagamentos",
                newName: "RecorrenciaDiaVencimento");

            migrationBuilder.AlterColumn<Guid>(
                name: "FamiliaId",
                table: "Tarefas",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "MembroDonoId",
                table: "Tarefas",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "TarefaId",
                table: "Pagamentos",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "FamiliaId",
                table: "Pagamentos",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataPagamento",
                table: "Pagamentos",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<int>(
                name: "Escopo",
                table: "Pagamentos",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MembroDonoId",
                table: "Tarefas");

            migrationBuilder.DropColumn(
                name: "Escopo",
                table: "Pagamentos");

            migrationBuilder.RenameColumn(
                name: "RecorrenciaDiaVencimento",
                table: "Pagamentos",
                newName: "RecorrenciaIntervalo");

            migrationBuilder.AlterColumn<Guid>(
                name: "FamiliaId",
                table: "Tarefas",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "TarefaId",
                table: "Pagamentos",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "FamiliaId",
                table: "Pagamentos",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataPagamento",
                table: "Pagamentos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<int[]>(
                name: "Recorrencia_DiasDaSemana",
                table: "Pagamentos",
                type: "integer[]",
                nullable: false,
                defaultValue: new int[0]);
        }
    }
}
