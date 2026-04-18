using Favly.Application.Categorias.DTOs;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Entities;
using Favly.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Categorias.Commands
{
    public class CriarCategoriaHandler
    {
        public static async Task<CategoriaResponse> Handle(
            CriarCategoriaCommand command,
            ICategoriaRepository repository,
            IUnitOfWork uow,
            CancellationToken ct)
        {
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
