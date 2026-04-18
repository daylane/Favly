using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Produtos.Commands.AtualizarProduto
{
    public record AtualizarProdutoCommand(
         Guid ProdutoId,
         string Nome,
         string? Marca,
         Guid CategoriaId,
         decimal QuantidadeMinima);
}
