using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Produtos.Queries.ListarProdutos
{
    public record ListarProdutosQuery(Guid GrupoId);
    public record ListarEstoqueBaixoQuery(Guid GrupoId);
    public record ObterProdutoPorIdQuery(Guid ProdutoId);

}
