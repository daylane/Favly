using Favly.Application.Mercados.DTOs;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Mercados.Commands.AtualizarMercado
{
    public class AtualizarMercadoHandler
    {
        public static async Task<MercadoResponse> Handle(
            AtualizarMercadoCommand command,
            IMercadoRepository repository,
            IUnitOfWork uow,
            CancellationToken ct)
        {
            var mercado = await repository.ObterPorIdAsync(command.MercadoId, ct);
            NotFoundException.When(mercado is null, "Mercado não encontrado.");

            mercado!.Atualizar(command.Nome, command.Endereco);
            repository.Atualizar(mercado);
            await uow.CommitAsync(ct);

            return MercadoResponse.FromEntity(mercado);
        }
    }

}
