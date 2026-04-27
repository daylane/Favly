namespace Favly.Application.Convites.Commands.AceitarConvite
{
    public record AceitarConviteCommand(Guid UsuarioId, string Codigo, string Apelido);
}
