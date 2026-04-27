using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Mercados.DTOs
{
    public record CriarMercadoRequest(string Nome, string? Endereco);
}
