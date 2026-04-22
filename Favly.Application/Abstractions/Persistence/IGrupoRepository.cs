using Favly.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Abstractions.Persistence
{
    public interface IGrupoRepository
    {
        Task<Grupo?> ObterPorIdAsync(Guid grupoId,
            CancellationToken cancellationToken = default);

        Task<Grupo?> ObterGrupoPorUsuarioIdAsync(Guid usuarioId,
           CancellationToken cancellationToken = default);

        Task<bool> UsuarioEhMembroAsync(Guid grupoId, Guid usuarioId,
            CancellationToken ct = default);

        Task<Grupo?> ObterPorIdComMembrosAsync(Guid grupoId,
            CancellationToken ct = default);

        Task<Grupo?> ObterPorCodigoConviteAsync(string codigo,
            CancellationToken ct = default);

        Task<IEnumerable<Grupo>> ListarGruposDoUsuarioAsync(Guid usuarioId,
            CancellationToken ct = default);

        Task AdicionarAsync(Grupo grupo,
            CancellationToken cancellationToken = default);

        void AtualizarAsync(Grupo grupo);
        Task<IEnumerable<(string Email, string Nome)>> ObterEmailsDoGrupoAsync(Guid grupoId, CancellationToken ct = default);

    }
}
