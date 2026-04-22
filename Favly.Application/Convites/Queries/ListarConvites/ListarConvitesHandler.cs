using Favly.Application.Abstractions.Persistence;
using Favly.Application.Convites.DTOs;
using Favly.Domain.Common.Exceptions;

namespace Favly.Application.Convites.Queries.ListarConvites
{
    public class ListarConvitesHandler
    {
        public static async Task<IEnumerable<ConviteResponse>> Handle(
            ListarConvitesQuery query,
            IConviteRepository conviteRepository,
            IGrupoRepository grupoRepository,
            CancellationToken ct)
        {
            var ehMembro = await grupoRepository.UsuarioEhMembroAsync(query.GrupoId, query.UsuarioId, ct);
            AcessoNegadoException.When(!ehMembro, "Você não é membro deste grupo.");

            var convites = await conviteRepository.ListarPorGrupoAsync(query.GrupoId, ct);
            return convites.Select(ConviteResponse.FromEntity);
        }
    }
}
