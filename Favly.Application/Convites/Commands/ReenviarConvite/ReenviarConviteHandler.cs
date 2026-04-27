using Favly.Application.Abstractions.Persistence;
using Favly.Application.Convites.DTOs;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Interfaces;

namespace Favly.Application.Convites.Commands.ReenviarConvite
{
    public class ReenviarConviteHandler
    {
        public static async Task<ConviteResponse> Handle(
            ReenviarConviteCommand command,
            IConviteRepository conviteRepository,
            IGrupoRepository grupoRepository,
            IEmailService emailService,
            IUnitOfWork uow,
            CancellationToken ct)
        {
            var ehMembro = await grupoRepository.UsuarioEhMembroAsync(command.GrupoId, command.UsuarioId, ct);
            AcessoNegadoException.When(!ehMembro, "Você não é membro deste grupo.");

            var convite = await conviteRepository.ObterPorIdAsync(command.ConviteId, ct);
            NotFoundException.When(convite is null, "Convite não encontrado.");
            DomainException.When(convite!.FamiliaId != command.GrupoId, "Este convite não pertence a este grupo.");

            // Gera novo código e estende a expiração por mais 7 dias
            convite.Reenviar();

            await uow.CommitAsync(ct);

            var grupo = await grupoRepository.ObterPorIdAsync(command.GrupoId, ct);

            await emailService.EnviarConviteAsync(
                email: convite.EmailConvidado.EnderecoEmail,
                grupoNome: grupo!.Nome,
                codigo: convite.Codigo,
                ct: ct);

            return ConviteResponse.FromEntity(convite);
        }
    }
}
