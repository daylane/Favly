namespace Favly.Application.Produtos.Commands.RemoverProduto
{
    public record RemoverProdutoCommand(Guid GrupoId, Guid UsuarioId, Guid ProdutoId);
}
