using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Categorias.DTOs
{
    public record CriarCategoriaRequest(string Nome, string? Icone);
}
