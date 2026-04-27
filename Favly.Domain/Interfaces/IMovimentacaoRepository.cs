using Favly.Domain.Entities;

namespace Favly.Domain.Interfaces
{
    public interface IMovimentacaoRepository
    {
        Task<IEnumerable<MovimentacaoDetalhada>> ListarPorProdutoAsync(Guid produtoId, CancellationToken ct = default);
        Task<IEnumerable<MovimentacaoDetalhada>> ListarPorGrupoAsync(Guid grupoId, int pagina, int tamanhoPagina, CancellationToken ct = default);
        Task AdicionarAsync(Movimentacao movimentacao, CancellationToken ct = default);
        Task<MovimentacaoDetalhada?> ObterDetalhadaPorIdAsync(Guid id, CancellationToken ct = default);

        Task<(decimal? Media, int TotalCompras)> ObterEstatisticasPrecoAsync(Guid produtoId, CancellationToken ct = default);
        Task<(Guid? MercadoId, decimal? MenorPrecoMedio)> ObterMercadoMaisBaratoAsync(Guid produtoId, CancellationToken ct = default);

        Task<IEnumerable<(Guid CategoriaId, decimal TotalGasto)>> ObterGastosPorCategoriaAsync(Guid grupoId, CancellationToken ct = default);
        Task<IEnumerable<(Guid MercadoId, decimal TotalGasto, int TotalCompras)>> ObterRankingMercadosAsync(Guid grupoId, CancellationToken ct = default);
        Task<Movimentacao?> ObterUltimaEntradaAsync(Guid grupoId, CancellationToken ct = default);
    }
}
