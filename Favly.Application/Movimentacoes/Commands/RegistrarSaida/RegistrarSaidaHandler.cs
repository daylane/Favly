using Favly.Application.Movimentacoes.DTOs;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Entities;
using Favly.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Wolverine;

namespace Favly.Application.Movimentacoes.Commands.RegistrarSaida
{
    public class RegistrarSaidaHandler
    {
        public static async Task<MovimentacaoResponse> Handle(
            RegistrarSaidaCommand command,
            IProdutoRepository produtoRepository,
            IMovimentacaoRepository movimentacaoRepository,
            IUnitOfWork uow,
            IMessageBus bus,
            CancellationToken ct)
        {
            var produto = await produtoRepository.ObterPorIdAsync(command.ProdutoId, ct);
            NotFoundException.When(produto is null, "Produto não encontrado.");

            produto!.RemoverEstoque(command.Quantidade); // dispara EstoqueBaixoEvent se necessário

            var movimentacao = Movimentacao.CriarSaida(
                command.GrupoId, command.ProdutoId, command.MembroId,
                command.Quantidade, command.Observacao);

            produtoRepository.Atualizar(produto);
            await movimentacaoRepository.AdicionarAsync(movimentacao, ct);
            await uow.CommitAsync(ct);

            foreach (var evento in produto.DomainEvents)
                await bus.PublishAsync(evento);

            produto.ClearDomainEvents();

            return MovimentacaoResponse.FromEntity(movimentacao);
        }
    }
}
