using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Categorias.Commands.AtualizarCategoria
{
    public record AtualizarCategoriaCommand(Guid CategoriaId, string Nome, string Icone);

}
