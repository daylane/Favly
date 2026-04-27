using Favly.Application.Abstractions.Persistence;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Interfaces;

namespace Favly.Application.Produtos.Commands.RemoverProduto
{
    public class RemoverProdutoHandler
    {
        public static async Task Handle(
            RemoverProdutoCommand command,
            IProdutoRepository repository,
            IGrupoRepository grupoRepository,
            IUnitOfWork uow,
            CancellationToken ct)
        {
            var ehMembro = await grupoRepository.UsuarioEhMembroAsync(command.GrupoId, command.UsuarioId, ct);
            AcessoNegadoException.When(!ehMembro, "Você não é membro deste grupo.");

            var produto = await repository.ObterPorIdAsync(command.ProdutoId, ct);
            NotFoundException.When(produto is null, "Produto não encontrado.");

            produto!.Desativar();
            repository.Atualizar(produto);
            await uow.CommitAsync(ct);
        }
    }
}
