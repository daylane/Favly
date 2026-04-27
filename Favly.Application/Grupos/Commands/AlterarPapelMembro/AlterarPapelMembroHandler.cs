using Favly.Application.Abstractions.Persistence;
using Favly.Application.Grupos.DTOs;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Interfaces;

namespace Favly.Application.Grupos.Commands.AlterarPapelMembro
{
    public class AlterarPapelMembroHandler
    {
        public static async Task<MembroResponse> Handle(
            AlterarPapelMembroCommand command,
            IGrupoRepository grupoRepository,
            IUnitOfWork uow,
            CancellationToken ct)
        {
            var grupo = await grupoRepository.ObterPorIdComMembrosAsync(command.GrupoId, ct);
            NotFoundException.When(grupo is null, "Grupo não encontrado.");

            grupo!.AlterarPapelMembro(command.AdminId, command.MembroId, command.NovoPapel);

            await uow.CommitAsync(ct);

            var membro = grupo.Membros.First(m => m.Id == command.MembroId);
            var membrosComUsuarios = await grupoRepository.ObterMembrosComUsuariosAsync(command.GrupoId, ct);
            return membrosComUsuarios.First(m => m.Id == command.MembroId);
        }
    }
}
