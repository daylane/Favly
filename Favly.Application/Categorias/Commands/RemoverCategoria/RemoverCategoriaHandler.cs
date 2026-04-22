using Favly.Application.Abstractions.Persistence;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Interfaces;

namespace Favly.Application.Categorias.Commands.RemoverCategoria
{
    public class RemoverCategoriaHandler
    {
        public static async Task Handle(
            RemoverCategoriaCommand command,
            ICategoriaRepository repository,
            IGrupoRepository grupoRepository,
            IUnitOfWork uow,
            CancellationToken ct)
        {
            var ehMembro = await grupoRepository.UsuarioEhMembroAsync(command.GrupoId, command.UsuarioId, ct);
            AcessoNegadoException.When(!ehMembro, "Você não é membro deste grupo.");

            var categoria = await repository.ObterPorIdAsync(command.CategoriaId, ct);
            NotFoundException.When(categoria is null, "Categoria não encontrada.");

            categoria!.Desativar();
            repository.Atualizar(categoria);
            await uow.CommitAsync(ct);
        }
    }
}
