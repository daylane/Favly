namespace Favly.Application.Grupos.DTOs
{
    public record ResumoGrupoResponse(
        int TotalProdutos,
        int ProdutosEstoqueBaixo,
        decimal? ValorUltimaCompra,
        DateTime? DataUltimaCompra);
}
