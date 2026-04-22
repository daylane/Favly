namespace Favly.Application.Grupos.Commands.EntrarPorCodigo
{
    public record EntrarPorCodigoCommand(Guid UsuarioId, string Codigo, string Apelido);
}
