using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Usuarios.Commands.CriarUsuario
{
    public class CriarUsuarioValidator : AbstractValidator<CriarUsuarioCommand>
    {
        public CriarUsuarioValidator() 
        {

            RuleFor(x => x.Nome)
                  .NotEmpty().WithMessage("Nome é obrigatório.")
                  .MaximumLength(100).WithMessage("Nome não pode ter mais de 100 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-mail é obrigatório.")
                .EmailAddress().WithMessage("Formato de e-mail inválido.");

            RuleFor(x => x.Senha)
                .NotEmpty().WithMessage("Senha é obrigatória.")
                .MinimumLength(8).WithMessage("Senha deve ter pelo menos 8 caracteres.")
                .Matches("[A-Z]").WithMessage("Senha deve conter ao menos uma letra maiúscula.")
                .Matches("[0-9]").WithMessage("Senha deve conter ao menos um número.");
        }
    }
}
