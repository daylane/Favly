using Favly.Application.Usuarios.DTOs;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Usuarios.Commands.AtualizarUsuario
{
    public class AtualizarUsuarioHandler
    {
        public static async Task<UsuarioResponse> Handle(
            AtualizarUsuarioCommand command,
            IUsuarioRepository repository,
            CancellationToken ct)
        {
            var usuario = await repository.ObterPorIdAsync(command.UsuarioId, ct);

            NotFoundException.When(usuario is null, "Usuário não encontrado.");

            usuario!.Atualizar(command.Nome, command.Avatar);
            repository.Atualizar(usuario);

            return UsuarioResponse.FromEntity(usuario);
        }
    }
}
