using Favly.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Categorias.DTOs
{
    public record CategoriaResponse(Guid Id, string Nome, string Icone, bool Ativo)
    {
        public static CategoriaResponse FromEntity(Categoria c) =>
            new(c.Id, c.Nome, c.Icone, c.Ativo);
    }
}
