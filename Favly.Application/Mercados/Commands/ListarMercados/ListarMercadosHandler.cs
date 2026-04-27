using Favly.Application.Abstractions.Persistence;
using Favly.Application.Mercados.DTOs;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Interfaces;

namespace Favly.Application.Mercados.Commands.ListarMercados
{
    public class ListarMercadosHandler
    {
        public static async Task<IEnumerable<MercadoResponse>> Handle(
            ListarMercadosQuery query,
            IMercadoRepository repository,
            IGrupoRepository grupoRepository,
            CancellationToken ct)
        {
            var ehMembro = await grupoRepository.UsuarioEhMembroAsync(query.GrupoId, query.UsuarioId, ct);
            AcessoNegadoException.When(!ehMembro, "Você não é membro deste grupo.");

            var mercados = await repository.ListarPorGrupoAsync(query.GrupoId, ct);
            return mercados.Select(MercadoResponse.FromEntity);
        }
    }
}
