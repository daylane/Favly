using Favly.Application.Abstractions.Persistence;
using Favly.Application.Grupos.DTOs;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Interfaces;

namespace Favly.Application.Grupos.Commands.AtualizarGrupo
{
    public class AtualizarGrupoHandler
    {
        public static async Task<GrupoResponse> Handle(
            AtualizarGrupoCommand command,
            IGrupoRepository grupoRepository,
            IUnitOfWork uow,
            CancellationToken ct)
        {
            var grupo = await grupoRepository.ObterPorIdComMembrosAsync(command.GrupoId, ct);
            NotFoundException.When(grupo is null, "Grupo não encontrado.");

            var ehMembro = await grupoRepository.UsuarioEhMembroAsync(command.GrupoId, command.UsuarioId, ct);
            AcessoNegadoException.When(!ehMembro, "Você não é membro deste grupo.");

            if (!string.IsNullOrWhiteSpace(command.Nome))
                grupo!.AlterarNome(command.Nome);

            if (!string.IsNullOrWhiteSpace(command.Avatar))
                grupo!.AlterarAvatar(command.Avatar);

            grupoRepository.AtualizarAsync(grupo!);
            await uow.CommitAsync(ct);

            return GrupoResponse.FromEntity(grupo!, command.UsuarioId);
        }
    }
}
