namespace Favly.Application.Produtos.DTOs
{
    public record ListarProdutosRequest(
        string? Nome = null,
        string? Marca = null,
        Guid? CategoriaId = null);
}
