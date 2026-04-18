using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Categorias.Commands
{

    public class CriarCategoriaValidator : AbstractValidator<CriarCategoriaCommand>
    {
        public CriarCategoriaValidator()
        {
            RuleFor(x => x.GrupoId).NotEmpty();
            RuleFor(x => x.Nome).NotEmpty().MaximumLength(50);
        }
    }
}
