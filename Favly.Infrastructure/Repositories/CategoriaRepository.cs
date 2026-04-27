using Favly.Domain.Entities;
using Favly.Domain.Interfaces;
using Favly.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Favly.Infrastructure.Repositories
{
    public class CategoriaRepository(FavlyDbContext _context) : ICategoriaRepository
    {
        public async Task<Categoria?> ObterPorIdAsync(Guid id, CancellationToken ct = default) =>
            await _context.Categorias.FirstOrDefaultAsync(c => c.Id == id, ct);

        public async Task<IEnumerable<Categoria>> ListarPorGrupoAsync(Guid grupoId, CancellationToken ct = default) =>
            await _context.Categorias
                .Where(c => c.GrupoId == grupoId && c.Ativo)
                .OrderBy(c => c.Nome)
                .ToListAsync(ct);

        public async Task<bool> ExisteNomeNoGrupoAsync(Guid grupoId, string nome, CancellationToken ct = default) =>
            await _context.Categorias
                .AnyAsync(c => c.GrupoId == grupoId && c.Nome.ToLower() == nome.ToLower() && c.Ativo, ct);

        public async Task AdicionarAsync(Categoria categoria, CancellationToken ct = default) =>
            await _context.Categorias.AddAsync(categoria, ct);

        public void Atualizar(Categoria categoria) =>
            _context.Categorias.Update(categoria);
    }
}