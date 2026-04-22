using Favly.Domain.Common.Enums;

namespace Favly.Application.Produtos.DTOs
{
    public record CriarProdutoRequest(
        Guid CategoriaId,
        string Nome,
        UnidadeMedida Unidade,
        decimal QuantidadeMinima,
        string? Marca = null);
}
