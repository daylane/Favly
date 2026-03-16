using Favly.Application.Usuarios.DTOs;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Usuarios.Queries.ObterUsuarioPorId
{
    public class ObterUsuarioPorIdHandler
    {
        public static async Task<UsuarioResponse> Handle(
            ObterUsuarioPorIdQuery query,
            IUsuarioRepository repository,
            CancellationToken ct)
        {
            var usuario = await repository.ObterPorIdAsync(query.UsuarioId, ct);

            NotFoundException.When(usuario is null, "Usuário não encontrado.");

            return UsuarioResponse.FromEntity(usuario!);
        }
    }
}
