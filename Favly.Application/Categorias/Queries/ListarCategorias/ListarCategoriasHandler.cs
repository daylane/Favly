using Favly.Application.Categorias.DTOs;
using Favly.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Categorias.Queries.ListarCategorias
{
    public class ListarCategoriasHandler
    {
        public static async Task<IEnumerable<CategoriaResponse>> Handle(
            ListarCategoriasQuery query,
            ICategoriaRepository repository,
            CancellationToken ct)
        {
            var categorias = await repository.ListarPorGrupoAsync(query.GrupoId, ct);
            return categorias.Select(CategoriaResponse.FromEntity);
        }
    }
}
