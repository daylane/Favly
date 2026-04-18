using Favly.Domain.Entities;

namespace Favly.Domain.Interfaces
{
    public interface ICategoriaRepository
    {
        Task<Categoria?> ObterPorIdAsync(Guid id, CancellationToken ct = default);
        Task<IEnumerable<Categoria>> ListarPorGrupoAsync(Guid grupoId, CancellationToken ct = default);
        Task<bool> ExisteNomeNoGrupoAsync(Guid grupoId, string nome, CancellationToken ct = default);
        Task AdicionarAsync(Categoria categoria, CancellationToken ct = default);
        void Atualizar(Categoria categoria);
    }
}