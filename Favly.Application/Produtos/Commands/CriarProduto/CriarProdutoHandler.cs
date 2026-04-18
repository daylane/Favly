using Favly.Application.Produtos.DTOs;
using Favly.Domain.Entities;
using Favly.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Produtos.Commands.CriarProduto
{
    public class CriarProdutoHandler
    {
        public static async Task<ProdutoResponse> Handle(
            CriarProdutoCommand command,
            IProdutoRepository repository,
            IUnitOfWork uow,
            CancellationToken ct)
        {
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
