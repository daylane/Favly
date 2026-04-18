using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Mercados.Commands.CriarMercado
{
    public record CriarMercadoCommand(Guid GrupoId, string Nome, string? Endereco = null);
}
