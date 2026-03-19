using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Auth.Commands.Login
{
    public class LoginValidator : AbstractValidator<LoginCommand>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-mail é obrigatório.")
                .EmailAddress().WithMessage("Formato de e-mail inválido.");

            RuleFor(x => x.Senha)
                .NotEmpty().WithMessage("Senha é obrigatória.");
        }
    }
}
