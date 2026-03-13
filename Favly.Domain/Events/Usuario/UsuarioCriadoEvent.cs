using Favly.Domain.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Domain.Events.Usuario
{
    public record UsuarioCriadoEvent(Guid UsuarioId, string Email, string Nome) : IDomainEvent;
}
