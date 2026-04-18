using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Mercados.Commands.AtualizarMercado
{
    public record AtualizarMercadoCommand(Guid MercadoId, string Nome, string? Endereco = null);
}
