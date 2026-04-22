namespace Favly.Application.Categorias.Commands.CriarCategoria
{
    public record CriarCategoriaCommand(Guid GrupoId, Guid UsuarioId, string Nome, string Icone = "📦");
}
