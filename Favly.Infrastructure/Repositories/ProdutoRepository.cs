using Favly.Domain.Entities;
using Favly.Domain.Interfaces;
using Favly.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Favly.Infrastructure.Repositories
{
    public class ProdutoRepository(FavlyDbContext _context) : IProdutoRepository
    {
        public async Task<Produto?> ObterPorIdAsync(Guid id, CancellationToken ct = default) =>
            await _context.Produtos.FirstOrDefaultAsync(p => p.Id == id, ct);

        public async Task<IEnumerable<Produto>> ListarPorGrupoAsync(Guid grupoId, CancellationToken ct = default) =>
            await _context.Produtos
                .Where(p => p.GrupoId == grupoId && p.Ativo)
                .OrderBy(p => p.Nome)
                .ToListAsync(ct);

        public async Task<IEnumerable<Produto>> ListarEstoqueBaixoAsync(Guid grupoId, CancellationToken ct = default) =>
            await _context.Produtos
                .Where(p => p.GrupoId == grupoId && p.Ativo && p.QuantidadeAtual <= p.QuantidadeMinima)
                .OrderBy(p => p.Nome)
                .ToListAsync(ct);

        public async Task AdicionarAsync(Produto produto, CancellationToken ct = default) =>
            await _context.Produtos.AddAsync(produto, ct);

        public void Atualizar(Produto produto) =>
            _context.Produtos.Update(produto);
    }
}