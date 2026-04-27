using Favly.Application.Abstractions.Persistence;
using Favly.Application.Produtos.DTOs;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Interfaces;

namespace Favly.Application.Produtos.Queries.ListarProdutos
{
    public class ListarProdutoHandler
    {
        public static async Task<IEnumerable<ProdutoResponse>> Handle(
            ListarProdutosQuery query,
            IProdutoRepository repository,
            IGrupoRepository grupoRepository,
            CancellationToken ct)
        {
            var ehMembro = await grupoRepository.UsuarioEhMembroAsync(query.GrupoId, query.UsuarioId, ct);
            AcessoNegadoException.When(!ehMembro, "Você não é membro deste grupo.");

            var filtros = new ProdutoFiltros(query.Nome, query.Marca, query.CategoriaId);
            var produtos = await repository.ListarPorGrupoAsync(query.GrupoId, filtros, ct);
            return produtos.Select(ProdutoResponse.FromEntity);
        }

        public static async Task<IEnumerable<ProdutoResponse>> Handle(
            ListarEstoqueBaixoQuery query,
            IProdutoRepository repository,
            IGrupoRepository grupoRepository,
            CancellationToken ct)
        {
            var ehMembro = await grupoRepository.UsuarioEhMembroAsync(query.GrupoId, query.UsuarioId, ct);
            AcessoNegadoException.When(!ehMembro, "Você não é membro deste grupo.");

            var produtos = await repository.ListarEstoqueBaixoAsync(query.GrupoId, ct);
            return produtos.Select(ProdutoResponse.FromEntity);
        }

        public static async Task<ProdutoResponse> Handle(
            ObterProdutoPorIdQuery query,
            IProdutoRepository repository,
            IGrupoRepository grupoRepository,
            CancellationToken ct)
        {
            var ehMembro = await grupoRepository.UsuarioEhMembroAsync(query.GrupoId, query.UsuarioId, ct);
            AcessoNegadoException.When(!ehMembro, "Você não é membro deste grupo.");

            var produto = await repository.ObterPorIdAsync(query.ProdutoId, ct);
            NotFoundException.When(produto is null, "Produto não encontrado.");
            return ProdutoResponse.FromEntity(produto!);
        }
    }
}
