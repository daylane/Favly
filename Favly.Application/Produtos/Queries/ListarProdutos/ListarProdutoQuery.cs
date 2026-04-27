namespace Favly.Application.Produtos.Queries.ListarProdutos
{
    public record ListarProdutosQuery(Guid GrupoId, Guid UsuarioId, string? Nome = null, string? Marca = null, Guid? CategoriaId = null);
    public record ListarEstoqueBaixoQuery(Guid GrupoId, Guid UsuarioId);
    public record ObterProdutoPorIdQuery(Guid GrupoId, Guid UsuarioId, Guid ProdutoId);
}
