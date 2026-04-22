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
    }
}