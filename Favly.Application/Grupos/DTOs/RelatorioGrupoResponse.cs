namespace Favly.Application.Grupos.DTOs
{
    public record RelatorioGrupoResponse(
        IEnumerable<GastoPorCategoriaItem> GastosPorCategoria,
        IEnumerable<RankingMercadoItem> RankingMercados);

    public record GastoPorCategoriaItem(
        Guid CategoriaId,
        string NomeCategoria,
        string IconeCategoria,
        decimal TotalGasto);

    public record RankingMercadoItem(
        Guid MercadoId,
        string NomeMercado,
        decimal TotalGasto,
        int TotalCompras);
}
