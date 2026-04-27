using Favly.Application.Abstractions.Persistence;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Interfaces;

namespace Favly.Application.Grupos.Commands.ExpulsarMembro
{
    public class ExpulsarMembroHandler
    {
        public static async Task Handle(
            ExpulsarMembroCommand command,
            IGrupoRepository grupoRepository,
            IUnitOfWork uow,
            CancellationToken ct)
        {
            var grupo = await grupoRepository.ObterPorIdComMembrosAsync(command.GrupoId, ct);
            NotFoundException.When(grupo is null, "Grupo não encontrado.");

            var membroRemovido = grupo!.ExpulsarMembro(command.AdminId, command.MembroId);

            grupoRepository.RemoverMembro(membroRemovido);
            await uow.CommitAsync(ct);
        }
    }
}
