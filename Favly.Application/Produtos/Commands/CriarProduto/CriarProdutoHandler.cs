using Favly.Application.Abstractions.Persistence;
using Favly.Application.Produtos.DTOs;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Entities;
using Favly.Domain.Interfaces;

namespace Favly.Application.Produtos.Commands.CriarProduto
{
    public class CriarProdutoHandler
    {
        public static async Task<ProdutoResponse> Handle(
            CriarProdutoCommand command,
            IProdutoRepository repository,
            IGrupoRepository grupoRepository,
            IUnitOfWork uow,
            CancellationToken ct)
        {
            var ehMembro = await grupoRepository.UsuarioEhMembroAsync(command.GrupoId, command.UsuarioId, ct);
            AcessoNegadoException.When(!ehMembro, "Você não é membro deste grupo.");

            var produto = Produto.Criar(
                command.GrupoId, command.CategoriaId,
                command.Nome, command.Unidade,
                command.QuantidadeMinima, command.Marca);

            await repository.AdicionarAsync(produto, ct);
            await uow.CommitAsync(ct);

            return ProdutoResponse.FromEntity(produto);
        }
    }
}
