using Favly.Application.Movimentacoes.DTOs;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Entities;
using Favly.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Wolverine;

namespace Favly.Application.Movimentacoes.Commands.RegistrarEntrada
{
    public class RegistrarEntradaHandler
    {
        public static async Task<MovimentacaoResponse> Handle(
            RegistrarEntradaCommand command,
            IProdutoRepository produtoRepository,
            IMovimentacaoRepository movimentacaoRepository,
            IUnitOfWork uow,
            IMessageBus bus,
            CancellationToken ct)
        {
            var produto = await produtoRepository.ObterPorIdAsync(command.ProdutoId, ct);
            NotFoundException.When(produto is null, "Produto não encontrado.");

            produto!.AdicionarEstoque(command.Quantidade, command.Preco, command.MercadoId);

            var movimentacao = Movimentacao.CriarEntrada(
                command.GrupoId, command.ProdutoId, command.MembroId,
                command.Quantidade, command.Preco, command.MercadoId, command.Observacao);

            produtoRepository.Atualizar(produto);
            await movimentacaoRepository.AdicionarAsync(movimentacao, ct);
            await uow.CommitAsync(ct);

            return MovimentacaoResponse.FromEntity(movimentacao);
        }
    }
}
