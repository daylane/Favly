using Favly.Application.Abstractions.Persistence;
using Favly.Application.Commands.Grupo.CriarGrupo;
using Favly.Domain.Common.Enums;
using Favly.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Handlers
{
    public static class CriarGrupoHandler
    {
        public static async Task<Guid> Handle(
        CriarGrupoCommand command,
        IGrupoRepository repository)
        {
            var novoGrupo = new Grupo(command.Nome, command.Avatar);

            novoGrupo.AdicionarMembro(
                command.UsuarioId,
                command.ApelidoCriador,
                PapelMembro.Administrador
            );

            await repository.AdicionarAsync(novoGrupo);

            return novoGrupo.Id;
        }
    }
}
