using Favly.Application.Abstractions.Persistence;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Interfaces;

namespace Favly.Application.Convites.Commands.RemoverConvite
{
    public class RemoverConviteHandler
    {
        public static async Task Handle(
            RemoverConviteCommand command,
            IConviteRepository conviteRepository,
            IGrupoRepository grupoRepository,
            IUnitOfWork uow,
            CancellationToken ct)
        {
            var ehMembro = await grupoRepository.UsuarioEhMembroAsync(command.GrupoId, command.UsuarioId, ct);
            AcessoNegadoException.When(!ehMembro, "Você não é membro deste grupo.");

            var convite = await conviteRepository.ObterPorIdAsync(command.ConviteId, ct);
            NotFoundException.When(convite is null, "Convite não encontrado.");
            DomainException.When(convite!.FamiliaId != command.GrupoId, "Este convite não pertence a este grupo.");

            convite.Remover();

            conviteRepository.Atualizar(convite);
            await uow.CommitAsync(ct);
        }
    }
}
