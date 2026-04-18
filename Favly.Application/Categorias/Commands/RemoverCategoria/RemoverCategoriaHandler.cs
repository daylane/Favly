using Favly.Domain.Common.Exceptions;
using Favly.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Categorias.Commands.RemoverCategoria
{

    public class RemoverCategoriaHandler
    {
        public static async Task Handle(
            RemoverCategoriaCommand command,
            ICategoriaRepository repository,
            IUnitOfWork uow,
            CancellationToken ct)
        {
            var categoria = await repository.ObterPorIdAsync(command.CategoriaId, ct);
            NotFoundException.When(categoria is null, "Categoria não encontrada.");

            categoria!.Desativar();
            repository.Atualizar(categoria);
            await uow.CommitAsync(ct);
        }
    }
}
