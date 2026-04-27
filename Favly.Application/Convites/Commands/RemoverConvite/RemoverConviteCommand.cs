using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Convites.Commands.RemoverConvite
{
    public record RemoverConviteCommand(Guid GrupoId, Guid UsuarioId, Guid ConviteId);
}
