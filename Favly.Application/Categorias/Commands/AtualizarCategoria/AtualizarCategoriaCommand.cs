namespace Favly.Application.Categorias.Commands.AtualizarCategoria
{
    public record AtualizarCategoriaCommand(Guid GrupoId, Guid UsuarioId, Guid CategoriaId, string Nome, string Icone);
}
