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

        public async Task<IEnumerable<Produto>> ListarPorGrupoAsync(
            Guid grupoId,
            ProdutoFiltros? filtros = null,
            CancellationToken ct = default)
        {
            var query = _context.Produtos
                .Where(p => p.GrupoId == grupoId && p.Ativo);

            if (!string.IsNullOrWhiteSpace(filtros?.Nome))
                query = query.Where(p => p.Nome.ToLower().Contains(filtros.Nome.ToLower()));

            if (!string.IsNullOrWhiteSpace(filtros?.Marca))
                query = query.Where(p => p.Marca != null && p.Marca.ToLower().Contains(filtros.Marca.ToLower()));

            if (filtros?.CategoriaId.HasValue == true)
                query = query.Where(p => p.CategoriaId == filtros.CategoriaId.Value);

            return await query.OrderBy(p => p.Nome).ToListAsync(ct);
        }

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