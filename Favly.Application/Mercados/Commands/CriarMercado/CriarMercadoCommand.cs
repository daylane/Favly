namespace Favly.Application.Mercados.Commands.CriarMercado
{
    public record CriarMercadoCommand(Guid GrupoId, Guid UsuarioId, string Nome, string? Endereco = null);
}
