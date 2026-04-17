using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Usuarios.Commands.ReenviarCodigoAtivacao
{
    public class ReenviarCodigoAtivacaoValidator : AbstractValidator<ReenviarCodigoAtivacaoCommand>
    {
        public ReenviarCodigoAtivacaoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-mail é obrigatório.")
                .EmailAddress().WithMessage("Formato de e-mail inválido.");
        }
    }
}
