using Favly.Application.Usuarios.DTOs;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Usuarios.Queries.ObterUsuarioPorEmail
{
    public class ObterUsuarioPorEmailHandler
    {
        public static async Task<UsuarioResponse> Handle(
           ObterUsuarioPorEmailQuery query,
           IUsuarioRepository repository,
           CancellationToken ct)
        {
            var usuario = await repository.ObterPorEmailAsync(query.Email, ct);

            NotFoundException.When(usuario is null, "Usuário não encontrado.");

            return UsuarioResponse.FromEntity(usuario!);
        }
    }
}
