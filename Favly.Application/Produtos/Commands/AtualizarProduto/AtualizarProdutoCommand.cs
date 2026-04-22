namespace Favly.Application.Produtos.Commands.AtualizarProduto
{
    public record AtualizarProdutoCommand(
        Guid GrupoId,
        Guid UsuarioId,
        Guid ProdutoId,
        string Nome,
        string? Marca,
        Guid CategoriaId,
        decimal QuantidadeMinima);
}
