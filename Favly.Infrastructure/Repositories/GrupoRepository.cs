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
        {
            throw new NotImplementedException();
        }


        public async Task<Grupo?> ObterGrupoPorUsuarioIdAsync(Guid usuarioId, CancellationToken cancellationToken = default)

            => await _context.Grupos.Where(x => x.Membros.Any(x => x.UsuarioId == usuarioId)).FirstAsync();

        public Task<Grupo?> ObterPorIdAsync(Guid grupoId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
