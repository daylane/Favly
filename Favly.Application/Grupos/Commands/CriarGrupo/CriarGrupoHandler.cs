using Favly.Application.Abstractions.Persistence;
using Favly.Application.Grupos.DTOs;
using Favly.Domain.Common.Enums;
using Favly.Domain.Entities;
using Favly.Domain.Interfaces;

namespace Favly.Application.Grupos.Commands.CriarGrupo
{
    public class CriarGrupoHandler
    {
        public static async Task<GrupoResponse> Handle(
            CriarGrupoCommand command,
            IGrupoRepository grupoRepository,
            IUsuarioRepository usuarioRepository,
            IUnitOfWork uow,
            CancellationToken ct)
        {
            var usuario = await usuarioRepository.ObterPorIdAsync(command.UsuarioId, ct);
            var grupo = new Grupo(command.Nome, command.Avatar);
            grupo.AdicionarMembro(command.UsuarioId, command.Apelido ?? usuario!.Nome, PapelMembro.Administrador);

            await grupoRepository.AdicionarAsync(grupo, ct);
            await uow.CommitAsync(ct);

            return GrupoResponse.FromEntity(grupo, command.UsuarioId);
        }
    }
}
