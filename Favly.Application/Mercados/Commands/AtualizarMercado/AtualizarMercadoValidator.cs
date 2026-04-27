using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Mercados.Commands.AtualizarMercado
{
    public class AtualizarMercadoValidator : AbstractValidator<AtualizarMercadoCommand>
    {
        public AtualizarMercadoValidator()
        {
            RuleFor(x => x.MercadoId).NotEmpty();
            RuleFor(x => x.Nome).NotEmpty().MaximumLength(100);
        }
    }
}
