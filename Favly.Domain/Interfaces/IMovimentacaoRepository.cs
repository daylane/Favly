using Favly.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Domain.Interfaces
{
    public interface IMovimentacaoRepository
    {
        Task<IEnumerable<Movimentacao>> ListarPorProdutoAsync(Guid produtoId, CancellationToken ct = default);
        Task<IEnumerable<Movimentacao>> ListarPorGrupoAsync(Guid grupoId, int pagina, int tamanhoPagina, CancellationToken ct = default);
        Task AdicionarAsync(Movimentacao movimentacao, CancellationToken ct = default);
    }
}
