namespace Favly.Application.Movimentacoes.Queries.ListarMovimentacoes
{
    public record ListarMovimentacoesQuery(
        Guid GrupoId,
        Guid UsuarioId,
        Guid? ProdutoId,
        int Pagina = 1,
        int Tamanho = 20);
}
