using Favly.Application.Abstractions.Persistence;
using Favly.Application.Grupos.DTOs;
using Favly.Domain.Common.Exceptions;

namespace Favly.Application.Grupos.Queries.ObterGrupo
{
    public class ObterGrupoHandler
    {
        public static async Task<GrupoResponse> Handle(
            ObterGrupoQuery query,
            IGrupoRepository grupoRepository,
            CancellationToken ct)
        {
            var ehMembro = await grupoRepository.UsuarioEhMembroAsync(query.GrupoId, query.UsuarioId, ct);
            AcessoNegadoException.When(!ehMembro, "Você não é membro deste grupo.");

            var grupo = await grupoRepository.ObterPorIdComMembrosAsync(query.GrupoId, ct);
            NotFoundException.When(grupo is null, "Grupo não encontrado.");

            return GrupoResponse.FromEntity(grupo!);
        }
    }
}
