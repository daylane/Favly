namespace Favly.Domain.Interfaces
{
    public record ProdutoFiltros(
        string? Nome = null,
        string? Marca = null,
        Guid? CategoriaId = null);
}
