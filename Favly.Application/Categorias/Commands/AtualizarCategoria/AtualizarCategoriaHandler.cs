using Favly.Application.Categorias.DTOs;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Categorias.Commands.AtualizarCategoria
{
    public class AtualizarCategoriaHandler
    {
        public static async Task<CategoriaResponse> Handle(
            AtualizarCategoriaCommand command,
            ICategoriaRepository repository,
            IUnitOfWork uow,
            CancellationToken ct)
        {
            var categoria = await repository.ObterPorIdAsync(command.CategoriaId, ct);
            NotFoundException.When(categoria is null, "Categoria não encontrada.");

            categoria!.Atualizar(command.Nome, command.Icone);
            repository.Atualizar(categoria);
            await uow.CommitAsync(ct);

            return CategoriaResponse.FromEntity(categoria);
        }
    }
}
