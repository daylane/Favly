namespace Favly.Application.Mercados.Commands.RemoverMercado
{
    public record RemoverMercadoCommand(Guid GrupoId, Guid UsuarioId, Guid MercadoId);
}
