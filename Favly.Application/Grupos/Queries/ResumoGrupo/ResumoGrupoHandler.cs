using Favly.Application.Abstractions.Persistence;
using Favly.Application.Grupos.DTOs;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Interfaces;

namespace Favly.Application.Grupos.Queries.ResumoGrupo
{
    public class ResumoGrupoHandler
    {
        public static async Task<ResumoGrupoResponse> Handle(
            ResumoGrupoQuery query,
            IGrupoRepository grupoRepository,
            IProdutoRepository produtoRepository,
            IMovimentacaoRepository movimentacaoRepository,
            CancellationToken ct)
        {
            var ehMembro = await grupoRepository.UsuarioEhMembroAsync(query.GrupoId, query.UsuarioId, ct);
            AcessoNegadoException.When(!ehMembro, "Você não é membro deste grupo.");

            var produtos = await produtoRepository.ListarPorGrupoAsync(query.GrupoId, ct: ct);
            var estoqueBaixo = await produtoRepository.ListarEstoqueBaixoAsync(query.GrupoId, ct);
            var ultimaEntrada = await movimentacaoRepository.ObterUltimaEntradaAsync(query.GrupoId, ct);

            return new ResumoGrupoResponse(
                produtos.Count(),
                estoqueBaixo.Count(),
                ultimaEntrada?.Preco,
                ultimaEntrada?.DataCriacao);
        }
    }
}
