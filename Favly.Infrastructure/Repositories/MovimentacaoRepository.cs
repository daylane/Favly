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
    }
}