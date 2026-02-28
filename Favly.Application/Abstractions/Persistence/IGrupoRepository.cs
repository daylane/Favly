using Favly.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Abstractions.Persistence
{
    public interface IGrupoRepository
    {
        Task<Grupo?> ObterPorIdAsync (Guid grupoId,
            CancellationToken cancellationToken = default);

        Task AdicionarAsync(Grupo grupo,
            CancellationToken cancellationToken = default);

        Task AtualizarAsync(Grupo grupo,
            CancellationToken cancellationToken = default);
    }
}
