namespace Favly.Application.Mercados.Commands.AtualizarMercado
{
    public record AtualizarMercadoCommand(Guid GrupoId, Guid UsuarioId, Guid MercadoId, string Nome, string? Endereco = null);
}
