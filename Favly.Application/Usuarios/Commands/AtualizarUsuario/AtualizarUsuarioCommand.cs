using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Usuarios.Commands.AtualizarUsuario
{
    public record AtualizarUsuarioCommand(Guid UsuarioId, string Nome, string? Avatar = null);
}
