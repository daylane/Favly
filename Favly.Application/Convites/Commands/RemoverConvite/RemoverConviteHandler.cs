using Favly.Application.Abstractions.Persistence;
using Favly.Application.Convites.Commands.CriarConvite;
using Favly.Application.Convites.DTOs;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Entities;
using Favly.Domain.Interfaces;
using Favly.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Convites.Commands.RemoverConvite
{
    public class RemoverConviteHandler
    {
        public static async Task<ConviteResponse> Handle(
          RemoverConviteCommand command,
          IConviteRepository conviteRepository,
          IGrupoRepository grupoRepository,
          IUnitOfWork uow,
          CancellationToken ct)
        {
            var grupo = await grupoRepository.ObterPorIdAsync(command.GrupoId, ct);
            NotFoundException.When(grupo is null, "Grupo não encontrado.");

            var convite = await conviteRepository.ObterPorIdAsync(command.Id, ct);
            NotFoundException.When(convite is null, "Convite não encontrado.");

            convite.Remover();

            conviteRepository.Atualizar(convite);
            await uow.CommitAsync(ct);

            return ConviteResponse.FromEntity(convite);
        }
    }
}
