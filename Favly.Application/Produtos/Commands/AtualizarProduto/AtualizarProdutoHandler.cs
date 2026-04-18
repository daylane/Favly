using Favly.Application.Produtos.DTOs;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Produtos.Commands.AtualizarProduto
{
    public class AtualizarProdutoHandler
    {
        public static async Task<ProdutoResponse> Handle(
            AtualizarProdutoCommand command,
            IProdutoRepository repository,
            IUnitOfWork uow,
            CancellationToken ct)
        {
            var produto = await repository.ObterPorIdAsync(command.ProdutoId, ct);
            NotFoundException.When(produto is null, "Produto não encontrado.");

            produto!.Atualizar(command.Nome, command.Marca, command.CategoriaId, command.QuantidadeMinima);
            repository.Atualizar(produto);
            await uow.CommitAsync(ct);

            return ProdutoResponse.FromEntity(produto);
        }
    }
}
