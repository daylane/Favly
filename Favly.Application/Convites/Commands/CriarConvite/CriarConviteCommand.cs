namespace Favly.Application.Convites.Commands.CriarConvite
{
    public record CriarConviteCommand(Guid GrupoId, Guid UsuarioId, string EmailConvidado);
}
