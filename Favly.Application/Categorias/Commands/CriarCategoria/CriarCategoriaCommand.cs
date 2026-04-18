using Favly.Application.Categorias.DTOs;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Entities;
using Favly.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Categorias.Commands
{
    public record CriarCategoriaCommand(Guid GrupoId, string Nome, string Icone = "📦");
}
