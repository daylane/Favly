namespace Favly.Application.Categorias.Commands.RemoverCategoria
{
    public record RemoverCategoriaCommand(Guid GrupoId, Guid UsuarioId, Guid CategoriaId);
}
