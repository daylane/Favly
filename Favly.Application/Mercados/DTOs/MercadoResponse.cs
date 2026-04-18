using Favly.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Mercados.DTOs
{
    public record MercadoResponse(Guid Id, string Nome, string? Endereco, bool Ativo)
    {
        public static MercadoResponse FromEntity(Mercado m) =>
            new(m.Id, m.Nome, m.Endereco, m.Ativo);
    }
}
