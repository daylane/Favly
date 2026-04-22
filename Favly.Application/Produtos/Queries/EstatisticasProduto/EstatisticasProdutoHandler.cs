using Favly.Application.Abstractions.Persistence;
using Favly.Application.Produtos.DTOs;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Interfaces;

namespace Favly.Application.Produtos.Queries.EstatisticasProduto
{
    public class EstatisticasProdutoHandler
    {
        public static async Task<EstatisticasProdutoResponse> Handle(
            EstatisticasProdutoQuery query,
            IProdutoRepository produtoRepository,
            IMovimentacaoRepository movimentacaoRepository,
            IMercadoRepository mercadoRepository,
            IGrupoRepository grupoRepository,
            CancellationToken ct)
        {
            var ehMembro = await grupoRepository.UsuarioEhMembroAsync(query.GrupoId, query.UsuarioId, ct);
            AcessoNegadoException.When(!ehMembro, "Você não é membro deste grupo.");

            var produto = await produtoRepository.ObterPorIdAsync(query.ProdutoId, ct);
            NotFoundException.When(produto is null, "Produto não encontrado.");

            var (media, totalCompras) = await movimentacaoRepository.ObterEstatisticasPrecoAsync(query.ProdutoId, ct);
            var (mercadoMaisBaratoId, menorPrecoMedio) = await movimentacaoRepository.ObterMercadoMaisBaratoAsync(query.ProdutoId, ct);

            string? mercadoMaisBaratoNome = null;
            if (mercadoMaisBaratoId.HasValue)
            {
                var mercado = await mercadoRepository.ObterPorIdAsync(mercadoMaisBaratoId.Value, ct);
                mercadoMaisBaratoNome = mercado?.Nome;
            }

            return new EstatisticasProdutoResponse(
                ProdutoId: produto!.Id,
                NomeProduto: produto.Nome,
                MediaPreco: media,
                TotalCompras: totalCompras,
                MercadoMaisBaratoId: mercadoMaisBaratoId,
                MercadoMaisBaratoNome: mercadoMaisBaratoNome,
                MenorPrecoMedio: menorPrecoMedio,
                UltimoPreco: produto.UltimoPreco,
                UltimoMercadoId: produto.UltimoMercadoId,
                UltimaCompra: produto.UltimaCompra);
        }
    }
}
