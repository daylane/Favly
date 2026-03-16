using Favly.Domain.Common.Exceptions;
using Favly.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Usuarios.Commands.DesativarUsuario
{
    public class DesativarUsuarioHandler
    {
        public static async Task Handle(
          DesativarUsuarioCommand command,
          IUsuarioRepository repository,
          CancellationToken ct)
        {
            var usuario = await repository.ObterPorIdAsync(command.UsuarioId, ct);

            NotFoundException.When(usuario is null, "Usuário não encontrado.");

            usuario!.Desativar();
            repository.Atualizar(usuario);
        }
    }
}
