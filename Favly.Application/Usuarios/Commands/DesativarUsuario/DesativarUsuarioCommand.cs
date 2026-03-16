using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Usuarios.Commands.DesativarUsuario
{
    public record DesativarUsuarioCommand(Guid UsuarioId);
}
