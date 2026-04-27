using Favly.Application.Abstractions.Persistence;
using Favly.Application.Categorias.DTOs;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Interfaces;

namespace Favly.Application.Categorias.Commands.AtualizarCategoria
{
    public class AtualizarCategoriaHandler
    {
        public static async Task<CategoriaResponse> Handle(
            AtualizarCategoriaCommand command,
            ICategoriaRepository repository,
            IGrupoRepository grupoRepository,
            IUnitOfWork uow,
            CancellationToken ct)
        {
            var ehMembro = await grupoRepository.UsuarioEhMembroAsync(command.GrupoId, command.UsuarioId, ct);
            AcessoNegadoException.When(!ehMembro, "Você não é membro deste grupo.");

            var categoria = await repository.ObterPorIdAsync(command.CategoriaId, ct);
            NotFoundException.When(categoria is null, "Categoria não encontrada.");

            categoria!.Atualizar(command.Nome, command.Icone);
            repository.Atualizar(categoria);
            await uow.CommitAsync(ct);

            return CategoriaResponse.FromEntity(categoria);
        }
    }
}
