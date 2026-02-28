using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Commands.Grupo.CriarGrupo
{
    public record CriarGrupoCommand(
    string Nome,
    string? Avatar,
    Guid UsuarioId,
    string ApelidoCriador);
}
