using Favly.Application.Abstractions.Persistence;
using Favly.Domain.Entities;
using Favly.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Infrastructure.Repositories
{
    public class GrupoRepository(FavlyDbContext _context) : IGrupoRepository
    {
        public async Task AdicionarAsync(Grupo grupo, CancellationToken cancellationToken = default)

            => await _context.Grupos.AddAsync(grupo, cancellationToken);

        public void  AtualizarAsync(Grupo grupo)
            => _context.Grupos.Update(grupo);

        public async Task<IEnumerable<(string Email, string Nome)>> ObterEmailsDoGrupoAsync(Guid grupoId, CancellationToken ct = default)
            => await _context.Membros
                .Where(m => m.FamiliaId == grupoId)
                .Join(_context.Usuarios,
                    m => m.UsuarioId,
                    u => u.Id,
                    (m, u) => new { Email = u.Email.EnderecoEmail, Nome = m.Apelido })
                .Select(x => ValueTuple.Create(x.Email, x.Nome))
                .ToListAsync(ct);


        public async Task<Grupo?> ObterGrupoPorUsuarioIdAsync(Guid usuarioId, CancellationToken cancellationToken = default)

            => await _context.Grupos.Where(x => x.Membros.Any(x => x.UsuarioId == usuarioId)).FirstAsync();

        public Task<Grupo?> ObterPorIdAsync(Guid grupoId, CancellationToken cancellationToken = default)
            => _context.Grupos.FirstOrDefaultAsync(g => g.Id == grupoId, cancellationToken);

        public Task<Grupo?> ObterPorIdComMembrosAsync(Guid grupoId, CancellationToken ct = default)
            => _context.Grupos.Include(g => g.Membros).FirstOrDefaultAsync(g => g.Id == grupoId, ct);

        public Task<Grupo?> ObterPorCodigoConviteAsync(string codigo, CancellationToken ct = default)
            => _context.Grupos.Include(g => g.Membros).FirstOrDefaultAsync(g => g.Convite == codigo, ct);

        public async Task<IEnumerable<Grupo>> ListarGruposDoUsuarioAsync(Guid usuarioId, CancellationToken ct = default)
            => await _context.Grupos
                .Where(g => g.Membros.Any(m => m.UsuarioId == usuarioId))
                .ToListAsync(ct);

        public Task<bool> UsuarioEhMembroAsync(Guid grupoId, Guid usuarioId, CancellationToken ct = default)
            => _context.Membros.AnyAsync(m => m.FamiliaId == grupoId && m.UsuarioId == usuarioId, ct);
    }
}
