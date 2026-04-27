using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Mercados.Commands.CriarMercado
{
    public class CriarMercadoValidator : AbstractValidator<CriarMercadoCommand>
    {
        public CriarMercadoValidator()
        {
            RuleFor(x => x.GrupoId).NotEmpty().WithMessage("GrupoId é obrigatório.");
            RuleFor(x => x.Nome).NotEmpty().WithMessage("Nome é obrigatório.")
                .MaximumLength(100).WithMessage("Nome não pode ter mais de 100 caracteres.");
        }
    }
}
