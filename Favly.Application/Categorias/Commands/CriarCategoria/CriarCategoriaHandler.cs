using Favly.Application.Abstractions.Persistence;
using Favly.Application.Categorias.DTOs;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Entities;
using Favly.Domain.Interfaces;

namespace Favly.Application.Categorias.Commands.CriarCategoria
{
    public class CriarCategoriaHandler
    {
        public static async Task<CategoriaResponse> Handle(
            CriarCategoriaCommand command,
            ICategoriaRepository repository,
            IGrupoRepository grupoRepository,
            IUnitOfWork uow,
            CancellationToken ct)
        {
            var ehMembro = await grupoRepository.UsuarioEhMembroAsync(command.GrupoId, command.UsuarioId, ct);
            AcessoNegadoException.When(!ehMembro, "Você não é membro deste grupo.");

            DomainException.When(
                await repository.ExisteNomeNoGrupoAsync(command.GrupoId, command.Nome, ct),
                "Já existe uma categoria com este nome no grupo.");

            var categoria = Categoria.Criar(command.GrupoId, command.Nome, command.Icone);
            await repository.AdicionarAsync(categoria, ct);
            await uow.CommitAsync(ct);

            return CategoriaResponse.FromEntity(categoria);
        }
    }
}
