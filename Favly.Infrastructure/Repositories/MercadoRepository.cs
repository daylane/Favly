using Favly.Domain.Entities;
using Favly.Domain.Interfaces;
using Favly.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Favly.Infrastructure.Repositories
{
    public class MercadoRepository(FavlyDbContext _context) : IMercadoRepository
    {
        public async Task<Mercado?> ObterPorIdAsync(Guid id, CancellationToken ct = default) =>
            await _context.Mercados.FirstOrDefaultAsync(m => m.Id == id, ct);

        public async Task<IEnumerable<Mercado>> ListarPorGrupoAsync(Guid grupoId, CancellationToken ct = default) =>
            await _context.Mercados
                .Where(m => m.GrupoId == grupoId && m.Ativo)
                .OrderBy(m => m.Nome)
                .ToListAsync(ct);

        public async Task AdicionarAsync(Mercado mercado, CancellationToken ct = default) =>
            await _context.Mercados.AddAsync(mercado, ct);

        public void Atualizar(Mercado mercado) =>
            _context.Mercados.Update(mercado);
    }
}