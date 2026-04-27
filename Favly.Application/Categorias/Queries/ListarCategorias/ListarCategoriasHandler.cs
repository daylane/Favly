using Favly.Application.Abstractions.Persistence;
using Favly.Application.Categorias.DTOs;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Interfaces;

namespace Favly.Application.Categorias.Queries.ListarCategorias
{
    public class ListarCategoriasHandler
    {
        public static async Task<IEnumerable<CategoriaResponse>> Handle(
            ListarCategoriasQuery query,
            ICategoriaRepository repository,
            IGrupoRepository grupoRepository,
            CancellationToken ct)
        {
            var ehMembro = await grupoRepository.UsuarioEhMembroAsync(query.GrupoId, query.UsuarioId, ct);
            AcessoNegadoException.When(!ehMembro, "Você não é membro deste grupo.");

            var categorias = await repository.ListarPorGrupoAsync(query.GrupoId, ct);
            return categorias.Select(CategoriaResponse.FromEntity);
        }
    }
}
