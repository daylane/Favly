using Favly.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Domain.Interfaces
{
    public interface IMercadoRepository
    {
        Task<Mercado?> ObterPorIdAsync(Guid id, CancellationToken ct = default);
        Task<IEnumerable<Mercado>> ListarPorGrupoAsync(Guid grupoId, CancellationToken ct = default);
        Task AdicionarAsync(Mercado mercado, CancellationToken ct = default);
        void Atualizar(Mercado mercado);
    }
}
