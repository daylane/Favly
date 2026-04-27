namespace Favly.Application.Grupos.Commands.AtualizarGrupo
{
    public record AtualizarGrupoCommand(Guid GrupoId, Guid UsuarioId, string? Nome, string? Avatar);
}
