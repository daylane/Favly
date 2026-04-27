using Favly.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Domain.Interfaces
{
    public interface IProdutoRepository
    {
        Task<Produto?> ObterPorIdAsync(Guid id, CancellationToken ct = default);
        Task<IEnumerable<Produto>> ListarPorGrupoAsync(Guid grupoId, ProdutoFiltros? filtros = null, CancellationToken ct = default);
        Task<IEnumerable<Produto>> ListarEstoqueBaixoAsync(Guid grupoId, CancellationToken ct = default);
        Task AdicionarAsync(Produto produto, CancellationToken ct = default);
        void Atualizar(Produto produto);
    }
}
