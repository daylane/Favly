using Favly.Domain.Common.Enums;
using Favly.Domain.Entities;
using Favly.Domain.Interfaces;
using Favly.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Favly.Infrastructure.Repositories
{
    public class MovimentacaoRepository(FavlyDbContext _context) : IMovimentacaoRepository
    {
        // ── Queries privadas reutilizáveis ────────────────────────────────────

        private IQueryable<MovimentacaoRaw> QueryComJoins(IQueryable<Movimentacao> base_) =>
            base_
                .Join(_context.Produtos,
                    m => m.ProdutoId, p => p.Id,
                    (m, p) => new { Mov = m, NomeProduto = p.Nome, p.Unidade })
                .Join(_context.Membros,
                    x => new { UserId = x.Mov.MembroId, GrupoId = x.Mov.GrupoId },
                    mb => new { UserId = mb.UsuarioId, GrupoId = mb.FamiliaId },
                    (x, mb) => new { x.Mov, x.NomeProduto, x.Unidade, NomeMembro = mb.Apelido })
                .GroupJoin(_context.Mercados,
                    x => x.Mov.MercadoId, mc => (Guid?)mc.Id,
                    (x, mcs) => new { x, mcs })
                .SelectMany(x => x.mcs.DefaultIfEmpty(), (x, mc) => new MovimentacaoRaw
                {
                    Id           = x.x.Mov.Id,
                    ProdutoId    = x.x.Mov.ProdutoId,
                    NomeProduto  = x.x.NomeProduto,
                    Unidade      = x.x.Unidade,
                    MembroId     = x.x.Mov.MembroId,
                    NomeMembro   = x.x.NomeMembro,
                    MercadoId    = x.x.Mov.MercadoId,
                    NomeMercado  = mc != null ? mc.Nome : null,
                    Tipo         = x.x.Mov.Tipo,
                    Quantidade   = x.x.Mov.Quantidade,
                    PrecoUnitario = x.x.Mov.PrecoUnitario,
                    ValorTotal   = x.x.Mov.ValorTotal,
                    Observacao   = x.x.Mov.Observacao,
                    DataCriacao  = x.x.Mov.DataCriacao
                });

        private static MovimentacaoDetalhada ToDetalhada(MovimentacaoRaw r) => new(
            r.Id,
            r.ProdutoId, r.NomeProduto, r.Unidade.Sigla(),
            r.MembroId, r.NomeMembro,
            r.MercadoId, r.NomeMercado,
            r.Tipo, r.Quantidade,
            r.PrecoUnitario, r.ValorTotal,
            r.Observacao, r.DataCriacao);

        // ── Listagens ─────────────────────────────────────────────────────────

        public async Task<IEnumerable<MovimentacaoDetalhada>> ListarPorProdutoAsync(
            Guid produtoId, CancellationToken ct = default)
        {
            var raw = await QueryComJoins(
                    _context.Movimentacoes
                        .Where(m => m.ProdutoId == produtoId)
                        .OrderByDescending(m => m.DataCriacao))
                .ToListAsync(ct);

            return raw.Select(ToDetalhada);
        }

        public async Task<IEnumerable<MovimentacaoDetalhada>> ListarPorGrupoAsync(
            Guid grupoId, int pagina, int tamanhoPagina, CancellationToken ct = default)
        {
            var raw = await QueryComJoins(
                    _context.Movimentacoes
                        .Where(m => m.GrupoId == grupoId)
                        .OrderByDescending(m => m.DataCriacao)
                        .Skip((pagina - 1) * tamanhoPagina)
                        .Take(tamanhoPagina))
                .ToListAsync(ct);

            return raw.Select(ToDetalhada);
        }

        public async Task AdicionarAsync(Movimentacao movimentacao, CancellationToken ct = default) =>
            await _context.Movimentacoes.AddAsync(movimentacao, ct);

        public async Task<MovimentacaoDetalhada?> ObterDetalhadaPorIdAsync(Guid id, CancellationToken ct = default)
        {
            var raw = await QueryComJoins(
                    _context.Movimentacoes.Where(m => m.Id == id))
                .FirstOrDefaultAsync(ct);

            return raw is null ? null : ToDetalhada(raw);
        }

        // ── Estatísticas ──────────────────────────────────────────────────────

        public async Task<(decimal? Media, int TotalCompras)> ObterEstatisticasPrecoAsync(
            Guid produtoId, CancellationToken ct = default)
        {
            var stats = await _context.Movimentacoes
                .Where(m => m.ProdutoId == produtoId
                         && m.Tipo == TipoMovimentacao.Entrada
                         && m.PrecoUnitario != null)
                .GroupBy(_ => 1)
                .Select(g => new { Media = g.Average(m => m.PrecoUnitario), Total = g.Count() })
                .FirstOrDefaultAsync(ct);

            return stats is null ? (null, 0) : (stats.Media, stats.Total);
        }

        public async Task<(Guid? MercadoId, decimal? MenorPrecoMedio)> ObterMercadoMaisBaratoAsync(
            Guid produtoId, CancellationToken ct = default)
        {
            var resultado = await _context.Movimentacoes
                .Where(m => m.ProdutoId == produtoId
                         && m.Tipo == TipoMovimentacao.Entrada
                         && m.PrecoUnitario != null
                         && m.MercadoId != null)
                .GroupBy(m => m.MercadoId)
                .Select(g => new { MercadoId = g.Key, Media = g.Average(m => m.PrecoUnitario) })
                .OrderBy(x => x.Media)
                .FirstOrDefaultAsync(ct);

            return resultado is null ? (null, null) : (resultado.MercadoId, resultado.Media);
        }

        public async Task<IEnumerable<(Guid CategoriaId, decimal TotalGasto)>> ObterGastosPorCategoriaAsync(
            Guid grupoId, CancellationToken ct = default) =>
            await _context.Movimentacoes
                .Where(m => m.GrupoId == grupoId
                         && m.Tipo == TipoMovimentacao.Entrada
                         && m.ValorTotal != null)
                .Join(_context.Produtos, m => m.ProdutoId, p => p.Id,
                    (m, p) => new { p.CategoriaId, Gasto = m.ValorTotal!.Value })
                .GroupBy(x => x.CategoriaId)
                .Select(g => ValueTuple.Create(g.Key, g.Sum(x => x.Gasto)))
                .ToListAsync(ct);

        public async Task<IEnumerable<(Guid MercadoId, decimal TotalGasto, int TotalCompras)>> ObterRankingMercadosAsync(
            Guid grupoId, CancellationToken ct = default) =>
            await _context.Movimentacoes
                .Where(m => m.GrupoId == grupoId
                         && m.Tipo == TipoMovimentacao.Entrada
                         && m.ValorTotal != null
                         && m.MercadoId != null)
                .GroupBy(m => m.MercadoId!.Value)
                .Select(g => new { MercadoId = g.Key, TotalGasto = g.Sum(m => m.ValorTotal!.Value), TotalCompras = g.Count() })
                .OrderByDescending(x => x.TotalGasto)
                .Select(x => ValueTuple.Create(x.MercadoId, x.TotalGasto, x.TotalCompras))
                .ToListAsync(ct);

        public async Task<Movimentacao?> ObterUltimaEntradaAsync(Guid grupoId, CancellationToken ct = default) =>
            await _context.Movimentacoes
                .Where(m => m.GrupoId == grupoId && m.Tipo == TipoMovimentacao.Entrada && m.PrecoUnitario != null)
                .OrderByDescending(m => m.DataCriacao)
                .FirstOrDefaultAsync(ct);

        // ── Tipo auxiliar de projeção SQL (evita anonymous types em métodos reutilizáveis) ──

        private sealed class MovimentacaoRaw
        {
            public Guid Id { get; init; }
            public Guid ProdutoId { get; init; }
            public string NomeProduto { get; init; } = "";
            public UnidadeMedida Unidade { get; init; }
            public Guid MembroId { get; init; }
            public string NomeMembro { get; init; } = "";
            public Guid? MercadoId { get; init; }
            public string? NomeMercado { get; init; }
            public TipoMovimentacao Tipo { get; init; }
            public decimal Quantidade { get; init; }
            public decimal? PrecoUnitario { get; init; }
            public decimal? ValorTotal { get; init; }
            public string? Observacao { get; init; }
            public DateTime DataCriacao { get; init; }
        }
    }
}
