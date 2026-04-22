namespace Favly.Application.Produtos.DTOs
{
    public record EstatisticasProdutoResponse(
        Guid ProdutoId,
        string NomeProduto,
        // Histórico de preços (calculado das movimentações)
        decimal? MediaPreco,
        int TotalCompras,
        // Mercado mais barato (menor preço médio nas entradas)
        Guid? MercadoMaisBaratoId,
        string? MercadoMaisBaratoNome,
        decimal? MenorPrecoMedio,
        // Última compra (armazenada no produto)
        decimal? UltimoPreco,
        Guid? UltimoMercadoId,
        DateTime? UltimaCompra);
}
