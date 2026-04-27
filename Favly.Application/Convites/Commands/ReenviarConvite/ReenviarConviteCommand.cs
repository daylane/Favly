namespace Favly.Application.Convites.Commands.ReenviarConvite
{
    public record ReenviarConviteCommand(Guid GrupoId, Guid UsuarioId, Guid ConviteId);
}
