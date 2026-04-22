using Favly.Domain.Common.Enums;
using Favly.Domain.Entities;
using Favly.Domain.Interfaces;
using Favly.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Favly.Infrastructure.Repositories
{
    public class MovimentacaoRepository(FavlyDbContext _context) : IMovimentacaoRepository
    {
        public async Task<IEnumerable<Movimentacao>> ListarPorProdutoAsync(Guid produtoId, CancellationToken ct = default) =>
            await _context.Movimentacoes
                .Where(m => m.ProdutoId == produtoId)
                .OrderByDescending(m => m.DataCriacao)
                .ToListAsync(ct);

        public async Task<IEnumerable<Movimentacao>> ListarPorGrupoAsync(
            Guid grupoId, int pagina, int tamanhoPagina, CancellationToken ct = default) =>
            await _context.Movimentacoes
                .Where(m => m.GrupoId == grupoId)
                .OrderByDescending(m => m.DataCriacao)
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToListAsync(ct);

        public async Task AdicionarAsync(Movimentacao movimentacao, CancellationToken ct = default) =>
            await _context.Movimentacoes.AddAsync(movimentacao, ct);

        public async Task<(decimal? Media, int TotalCompras)> ObterEstatisticasPrecoAsync(Guid produtoId, CancellationToken ct = default)
        {
            var stats = await _context.Movimentacoes
                .Where(m => m.ProdutoId == produtoId
                         && m.Tipo == TipoMovimentacao.Entrada
                         && m.Preco != null)
                .GroupBy(_ => 1)
                .Select(g => new { Media = g.Average(m => m.Preco), Total = g.Count() })
                .FirstOrDefaultAsync(ct);

            return stats is null ? (null, 0) : (stats.Media, stats.Total);
        }

        public async Task<(Guid? MercadoId, decimal? MenorPrecoMedio)> ObterMercadoMaisBaratoAsync(Guid produtoId, CancellationToken ct = default)
        {
            var resultado = await _context.Movimentacoes
                .Where(m => m.ProdutoId == produtoId
                         && m.Tipo == TipoMovimentacao.Entrada
                         && m.Preco != null
                         && m.MercadoId != null)
                .GroupBy(m => m.MercadoId)
                .Select(g => new { MercadoId = g.Key, Media = g.Average(m => m.Preco) })
                .OrderBy(x => x.Media)
                .FirstOrDefaultAsync(ct);

            return resultado is null ? (null, null) : (resultado.MercadoId, resultado.Media);
        }

        public async Task<IEnumerable<(Guid CategoriaId, decimal TotalGasto)>> ObterGastosPorCategoriaAsync(Guid grupoId, CancellationToken ct = default)
            => await _context.Movimentacoes
                .Where(m => m.GrupoId == grupoId
                         && m.Tipo == TipoMovimentacao.Entrada
                         && m.Preco != null)
                .Join(_context.Produtos,
                    m => m.ProdutoId,
                    p => p.Id,
                    (m, p) => new { p.CategoriaId, Gasto = m.Quantidade * m.Preco!.Value })
                .GroupBy(x => x.CategoriaId)
                .Select(g => ValueTuple.Create(g.Key, g.Sum(x => x.Gasto)))
                .ToListAsync(ct);

        public async Task<IEnumerable<(Guid MercadoId, decimal TotalGasto, int TotalCompras)>> ObterRankingMercadosAsync(Guid grupoId, CancellationToken ct = default)
            => await _context.Movimentacoes
                .Where(m => m.GrupoId == grupoId
                         && m.Tipo == TipoMovimentacao.Entrada
                         && m.Preco != null
                         && m.MercadoId != null)
                .GroupBy(m => m.MercadoId!.Value)
                .Select(g => new { MercadoId = g.Key, TotalGasto = g.Sum(m => m.Quantidade * m.Preco!.Value), TotalCompras = g.Count() })
                .OrderByDescending(x => x.TotalGasto)
                .Select(x => ValueTuple.Create(x.MercadoId, x.TotalGasto, x.TotalCompras))
                .ToListAsync(ct);

        public async Task<Movimentacao?> ObterUltimaEntradaAsync(Guid grupoId, CancellationToken ct = default)
            => await _context.Movimentacoes
                .Where(m => m.GrupoId == grupoId && m.Tipo == TipoMovimentacao.Entrada && m.Preco != null)
                .OrderByDescending(m => m.DataCriacao)
                .FirstOrDefaultAsync(ct);
    }
}
