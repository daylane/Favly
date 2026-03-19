using Favly.Domain.Common.Exceptions;
using Favly.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Usuarios.Commands.AtivarUsuario
{
    public class AtivarUsuarioHandler
    {
        public static async Task<bool> Handle(
               AtivarUsuarioCommand command,
               IUsuarioRepository repository,
               CancellationToken ct)
        {
            var usuario = await repository.ObterPorIdAsync(command.UsuarioId, ct);

            NotFoundException.When(usuario is null, "Usuário não encontrado.");

            usuario!.Ativar(command.CodigoAtivacao);
            repository.Atualizar(usuario);

            return true;
        }
    }
}
