namespace Favly.Application.Movimentacoes.DTOs
{
    public record RegistrarSaidaRequest(Guid ProdutoId, decimal Quantidade, string? Observacao);
}
