using Favly.Application.Mercados.DTOs;
using Favly.Domain.Entities;
using Favly.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Mercados.Commands.CriarMercado
{
    public class CriarMercadoHandler
    {
        public static async Task<MercadoResponse> Handle(
            CriarMercadoCommand command,
            IMercadoRepository repository,
            IUnitOfWork uow,
            CancellationToken ct)
        {
            var mercado = Mercado.Criar(command.GrupoId, command.Nome, command.Endereco);

            await repository.AdicionarAsync(mercado, ct);
            await uow.CommitAsync(ct);

            return MercadoResponse.FromEntity(mercado);
        }
    }
}
