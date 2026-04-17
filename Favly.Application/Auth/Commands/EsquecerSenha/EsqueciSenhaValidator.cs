using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Auth.Commands.EsquecerSenha
{
    public class EsqueciSenhaValidator : AbstractValidator<EsqueciSenhaCommand>
    {
        public EsqueciSenhaValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-mail é obrigatório.")
                .EmailAddress().WithMessage("Formato de e-mail inválido.");
        }
    }
}
