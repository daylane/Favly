using Favly.Application.Abstractions.Persistence;
using Favly.Application.Grupos.DTOs;
using Favly.Domain.Common.Enums;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Interfaces;

namespace Favly.Application.Grupos.Commands.EntrarPorCodigo
{
    public class EntrarPorCodigoHandler
    {
        public static async Task<GrupoResponse> Handle(
            EntrarPorCodigoCommand command,
            IGrupoRepository grupoRepository,
            IUnitOfWork uow,
            CancellationToken ct)
        {
            var grupo = await grupoRepository.ObterPorCodigoConviteAsync(command.Codigo, ct);
            NotFoundException.When(grupo is null, "Código de convite inválido.");

            grupo!.AdicionarMembro(command.UsuarioId, command.Apelido, PapelMembro.Usuario);

            grupoRepository.AtualizarAsync(grupo);
            await uow.CommitAsync(ct);

            return GrupoResponse.FromEntity(grupo);
        }
    }
}
