using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Auth.Commands.RedefinirSenha
{
    public class RedefinirSenhaValidator : AbstractValidator<RedefinirSenhaCommand>
    {
        public RedefinirSenhaValidator()
        {
            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Token é obrigatório.");

            RuleFor(x => x.NovaSenha)
                .NotEmpty().WithMessage("Nova senha é obrigatória.")
                .MinimumLength(8).WithMessage("Senha deve ter pelo menos 8 caracteres.")
                .Matches("[A-Z]").WithMessage("Senha deve conter ao menos uma letra maiúscula.")
                .Matches("[0-9]").WithMessage("Senha deve conter ao menos um número.");

            RuleFor(x => x.ConfirmacaoSenha)
                .NotEmpty().WithMessage("Confirmação de senha é obrigatória.")
                .Equal(x => x.NovaSenha).WithMessage("As senhas não conferem.");
        }
    }
}
