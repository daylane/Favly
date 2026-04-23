using Favly.Application.Abstractions.Persistence;
using Favly.Domain.Entities;
using Favly.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Favly.Infrastructure.Repositories
{
    public class ConviteRepository(FavlyDbContext _context) : IConviteRepository
    {
        public Task<Convite?> ObterPorCodigoAsync(string codigo, CancellationToken ct = default)
            => _context.Convites.FirstOrDefaultAsync(c => c.Codigo == codigo, ct);

        public async Task<IEnumerable<Convite>> ListarPorGrupoAsync(Guid grupoId, CancellationToken ct = default)
            => await _context.Convites
                .Where(c => c.FamiliaId == grupoId && c.Status != Domain.Common.Enums.StatusConvite.Removido)
                .OrderByDescending(c => c.DataCriacao)
                .ToListAsync(ct);

        public async Task AdicionarAsync(Convite convite, CancellationToken ct = default)
            => await _context.Convites.AddAsync(convite, ct);

        public void Atualizar(Convite convite)
            => _context.Convites.Update(convite);

        public async Task<Convite?> ObterPorIdAsync(Guid id, CancellationToken ct = default)
            => await _context.Convites.FindAsync(id);
    }
}
