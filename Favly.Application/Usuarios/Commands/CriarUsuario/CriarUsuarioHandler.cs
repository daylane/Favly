using Favly.Application.Usuarios.DTOs;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Entities;
using Favly.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Usuarios.Commands.CriarUsuario
{
    public class CriarUsuarioHandler
    {
        public static async Task<UsuarioResponse> Handle(
           CriarUsuarioCommand command,
           IUsuarioRepository repository,
           IPasswordHasher hasher,
           CancellationToken ct)
        {
            DomainException.When(
                await repository.EmailExisteAsync(command.Email, ct),
                "Este e-mail já está em uso.");

            var hash = hasher.Hash(command.Senha);
            var usuario = Usuario.Criar(command.Email, command.Nome, hash, command.Avatar);

            await repository.AdicionarAsync(usuario, ct);

            return UsuarioResponse.FromEntity(usuario);
        }
    }
}
