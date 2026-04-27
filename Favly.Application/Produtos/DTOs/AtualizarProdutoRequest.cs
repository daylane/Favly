using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Produtos.DTOs
{
    public record AtualizarProdutoRequest(string Nome, string? Marca, Guid CategoriaId, decimal QuantidadeMinima);
}
