using Favly.Application.Abstractions.Persistence;
using Favly.Application.Grupos.DTOs;

namespace Favly.Application.Grupos.Queries.ListarGrupos
{
    public class ListarGruposHandler
    {
        public static async Task<IEnumerable<GrupoResponse>> Handle(
            ListarGruposQuery query,
            IGrupoRepository grupoRepository,
            CancellationToken ct)
        {
            var grupos = await grupoRepository.ListarGruposDoUsuarioAsync(query.UsuarioId, ct);
            return grupos.Select(GrupoResponse.FromEntity);
        }
    }
}
