namespace Favly.Application.Grupos.Commands.CriarGrupo
{
    public record CriarGrupoCommand(Guid UsuarioId, string Nome, string? Avatar, string Apelido);
}
