using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Usuarios.Commands.AtivarUsuario
{
    public record AtivarUsuarioCommand(Guid UsuarioId, string CodigoAtivacao);
}
