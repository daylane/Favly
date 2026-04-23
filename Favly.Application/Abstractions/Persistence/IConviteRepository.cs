using Favly.Domain.Entities;

namespace Favly.Application.Abstractions.Persistence
{
    public interface IConviteRepository
    {
        Task<Convite?> ObterPorCodigoAsync(string codigo, CancellationToken ct = default);
        Task<Convite?> ObterPorIdAsync(Guid id, CancellationToken ct = default);
        Task<IEnumerable<Convite>> ListarPorGrupoAsync(Guid grupoId, CancellationToken ct = default);
        Task AdicionarAsync(Convite convite, CancellationToken ct = default);
        void Atualizar(Convite convite);
    }
}
