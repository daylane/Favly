using Favly.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Produtos.Commands.CriarProduto
{
    public record CriarProdutoCommand(
        Guid GrupoId,
        Guid CategoriaId,
        string Nome,
        UnidadeMedida Unidade,
        decimal QuantidadeMinima,
        string? Marca = null);
}
