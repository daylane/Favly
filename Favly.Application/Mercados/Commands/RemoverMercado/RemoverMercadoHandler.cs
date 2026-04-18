using Favly.Domain.Common.Exceptions;
using Favly.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Mercados.Commands.RemoverMercado
{
    public class RemoverMercadoHandler
    {
        public static async Task Handle(
            RemoverMercadoCommand command,
            IMercadoRepository repository,
            IUnitOfWork uow,
            CancellationToken ct)
        {
            var mercado = await repository.ObterPorIdAsync(command.MercadoId, ct);
            NotFoundException.When(mercado is null, "Mercado não encontrado.");

            mercado!.Desativar();
            repository.Atualizar(mercado);
            await uow.CommitAsync(ct);
        }
    }
}
