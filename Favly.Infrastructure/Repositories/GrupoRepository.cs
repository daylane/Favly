using Favly.Application.Abstractions.Persistence;
using Favly.Application.Grupos.DTOs;
using Favly.Domain.Common.Enums;
using Favly.Domain.Entities;
using Favly.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Favly.Infrastructure.Repositories
{
    public class GrupoRepository(FavlyDbContext _context) : IGrupoRepository
    {
        public async Task AdicionarAsync(Grupo grupo, CancellationToken cancellationToken = default)
            => await _context.Grupos.AddAsync(grupo, cancellationToken);

        public void AtualizarAsync(Grupo grupo)
        {
            // PROBLEMA: o EF Core usa ValueGeneratedOnAdd para Guid. Quando o novo
            // Membro tem Id = Guid.NewGuid() (≠ Guid.Empty), o EF assume "entidade
            // existente" e gera UPDATE → 0 rows → DbUpdateConcurrencyException.
            //
            // SOLUÇÃO: desabilitar AutoDetectChanges antes de iterar para impedir
            // que o ChangeTracker faça um scan de navegação e trackei o novo Membro
            // como Unchanged. Em seguida, forçar State = Added explicitamente,
            // o que sobrepõe qualquer estado anterior e garante INSERT.
            var autoDetect = _context.ChangeTracker.AutoDetectChangesEnabled;
            _context.ChangeTracker.AutoDetectChangesEnabled = false;
            try
            {
                foreach (var membro in grupo.Membros)
                {
                    if (_context.Membros.Local.All(m => m.Id != membro.Id))
                        _context.Entry(membro).State = EntityState.Added;
                }
            }
            finally
            {
                _context.ChangeTracker.AutoDetectChangesEnabled = autoDetect;
            }
        }

        public void RemoverMembro(Membro membro)
            => _context.Membros.Remove(membro);

        public Task<bool> UsuarioPossuiOutrosGruposAsync(Guid usuarioId, Guid grupoIdAtual, CancellationToken ct = default)
            => _context.Membros.AnyAsync(
                m => m.UsuarioId == usuarioId && m.FamiliaId != grupoIdAtual, ct);

        public async Task<IEnumerable<(string Email, string Nome)>> ObterEmailsDoGrupoAsync(Guid grupoId, CancellationToken ct = default)
            => await _context.Membros
                .Where(m => m.FamiliaId == grupoId)
                .Join(_context.Usuarios,
                    m => m.UsuarioId,
                    u => u.Id,
                    (m, u) => new { Email = u.Email.EnderecoEmail, Nome = m.Apelido })
                .Select(x => ValueTuple.Create(x.Email, x.Nome))
                .ToListAsync(ct);

        public async Task<IEnumerable<MembroResponse>> ObterMembrosComUsuariosAsync(
     Guid grupoId, CancellationToken ct = default)
     => await _context.Membros
         .Where(m => m.FamiliaId == grupoId)
         .Join(_context.Usuarios,
             m => m.UsuarioId,
             u => u.Id,
             (m, u) => new MembroResponse(
                 m.Id,
                 m.UsuarioId,
                 u.Nome,
                 u.Avatar,
                 m.Apelido,
                 m.Role.ToString(),
                 m.DataCriacao
             ))
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
                .Include(g => g.Membros)
                .Where(g => g.Membros.Any(m => m.UsuarioId == usuarioId))
                .ToListAsync(ct);

        public Task<bool> UsuarioEhMembroAsync(Guid grupoId, Guid usuarioId, CancellationToken ct = default)
            => _context.Membros.AnyAsync(m => m.FamiliaId == grupoId && m.UsuarioId == usuarioId, ct);
    }
}
