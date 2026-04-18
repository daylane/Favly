using Favly.Domain.Common.Exceptions;
using Favly.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Produtos.Commands.RemoverProduto
{
    public class RemoverProdutoHandler
    {
        public static async Task Handle(
            RemoverProdutoCommand command,
            IProdutoRepository repository,
            IUnitOfWork uow,
            CancellationToken ct)
        {
            var produto = await repository.ObterPorIdAsync(command.ProdutoId, ct);
            NotFoundException.When(produto is null, "Produto não encontrado.");

            produto!.Desativar();
            repository.Atualizar(produto);
            await uow.CommitAsync(ct);
        }
    }
}
