using Favly.Application.Mercados.DTOs;
using Favly.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Mercados.Commands.ListarMercados
{
    public class ListarMercadosHandler
    {
        public static async Task<IEnumerable<MercadoResponse>> Handle(
            ListarMercadosQuery query,
            IMercadoRepository repository,
            CancellationToken ct)
        {
            var mercados = await repository.ListarPorGrupoAsync(query.GrupoId, ct);
            return mercados.Select(MercadoResponse.FromEntity);
        }
    }
}
