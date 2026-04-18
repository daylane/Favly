using Favly.Application.Produtos.DTOs;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Produtos.Queries.ListarProdutos
{
  
    public class ListarProdutoHandlers
    {
        public static async Task<IEnumerable<ProdutoResponse>> Handle(
            ListarProdutosQuery query,
            IProdutoRepository repository,
            CancellationToken ct)
        {
            var produtos = await repository.ListarPorGrupoAsync(query.GrupoId, ct);
            return produtos.Select(ProdutoResponse.FromEntity);
        }

        public static async Task<IEnumerable<ProdutoResponse>> Handle(
            ListarEstoqueBaixoQuery query,
            IProdutoRepository repository,
            CancellationToken ct)
        {
            var produtos = await repository.ListarEstoqueBaixoAsync(query.GrupoId, ct);
            return produtos.Select(ProdutoResponse.FromEntity);
        }

        public static async Task<ProdutoResponse> Handle(
            ObterProdutoPorIdQuery query,
            IProdutoRepository repository,
            CancellationToken ct)
        {
            var produto = await repository.ObterPorIdAsync(query.ProdutoId, ct);
            NotFoundException.When(produto is null, "Produto não encontrado.");
            return ProdutoResponse.FromEntity(produto!);
        }
    }
}
