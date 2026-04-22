using Favly.Application.Abstractions.Persistence;
using Favly.Application.Movimentacoes.DTOs;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Interfaces;

namespace Favly.Application.Movimentacoes.Queries.ListarMovimentacoes
{
    public class ListarMovimentacoesHandler
    {
        public static async Task<IEnumerable<MovimentacaoResponse>> Handle(
            ListarMovimentacoesQuery query,
            IMovimentacaoRepository movimentacaoRepository,
            IGrupoRepository grupoRepository,
            CancellationToken ct)
        {
            var ehMembro = await grupoRepository.UsuarioEhMembroAsync(query.GrupoId, query.UsuarioId, ct);
            AcessoNegadoException.When(!ehMembro, "Você não é membro deste grupo.");

            IEnumerable<Favly.Domain.Entities.Movimentacao> movimentacoes;

            if (query.ProdutoId.HasValue)
                movimentacoes = await movimentacaoRepository.ListarPorProdutoAsync(query.ProdutoId.Value, ct);
            else
                movimentacoes = await movimentacaoRepository.ListarPorGrupoAsync(query.GrupoId, query.Pagina, query.Tamanho, ct);

            return movimentacoes.Select(MovimentacaoResponse.FromEntity);
        }
    }
}
