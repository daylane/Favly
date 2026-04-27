using Favly.Domain.Common.Enums;

namespace Favly.Application.Produtos.Commands.CriarProduto
{
    public record CriarProdutoCommand(
        Guid GrupoId,
        Guid UsuarioId,
        Guid CategoriaId,
        string Nome,
        UnidadeMedida Unidade,
        decimal QuantidadeMinima,
        string? Marca = null);
}
