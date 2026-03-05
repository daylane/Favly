using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Favly.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Convites",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FamiliaId = table.Column<Guid>(type: "uuid", nullable: false),
                    EmailConvidado = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Codigo = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    DataExpiracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Convites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Grupos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Convite = table.Column<string>(type: "character(10)", fixedLength: true, maxLength: 10, nullable: false),
                    Avatar = table.Column<string>(type: "text", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grupos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pagamentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FamiliaId = table.Column<Guid>(type: "uuid", nullable: false),
                    MembroResponsavelId = table.Column<Guid>(type: "uuid", nullable: false),
                    TarefaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Titulo = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Montante = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Moeda = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false, defaultValue: "BRL"),
                    Recorrencia_DiasDaSemana = table.Column<int[]>(type: "integer[]", nullable: false),
                    RecorrenciaTipo = table.Column<int>(type: "integer", nullable: false),
                    RecorrenciaIntervalo = table.Column<int>(type: "integer", nullable: false),
                    DataVencimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataPagamento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagamentos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tarefas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TarefaPaiId = table.Column<Guid>(type: "uuid", nullable: true),
                    FamiliaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Titulo = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Escopo = table.Column<int>(type: "integer", nullable: false),
                    ProximaOcorrencia = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Recorrencia_DiasDaSemana = table.Column<int[]>(type: "integer[]", nullable: false),
                    RecorrenciaTipo = table.Column<int>(type: "integer", nullable: false),
                    RecorrenciaIntervalo = table.Column<int>(type: "integer", nullable: false),
                    MembrosAtribuidosIds = table.Column<Guid[]>(type: "uuid[]", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarefas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tarefas_Tarefas_TarefaPaiId",
                        column: x => x.TarefaPaiId,
                        principalTable: "Tarefas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Hash = table.Column<string>(type: "text", nullable: false),
                    Avatar = table.Column<string>(type: "text", nullable: false),
                    CodigoAtivacao = table.Column<string>(type: "text", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Membros",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FamiliaId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    Apelido = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Membros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Membros_Grupos_FamiliaId",
                        column: x => x.FamiliaId,
                        principalTable: "Grupos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Membros_FamiliaId",
                table: "Membros",
                column: "FamiliaId");

            migrationBuilder.CreateIndex(
                name: "IX_Tarefas_TarefaPaiId",
                table: "Tarefas",
                column: "TarefaPaiId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Convites");

            migrationBuilder.DropTable(
                name: "Membros");

            migrationBuilder.DropTable(
                name: "Pagamentos");

            migrationBuilder.DropTable(
                name: "Tarefas");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Grupos");
        }
    }
}
