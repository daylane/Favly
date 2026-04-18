using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Mercados.DTOs
{
    public record AtualizarMercadoRequest(string Nome, string? Endereco);
}
