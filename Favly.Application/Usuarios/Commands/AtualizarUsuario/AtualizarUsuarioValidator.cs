using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Usuarios.Commands.AtualizarUsuario
{
    public class AtualizarUsuarioValidator : AbstractValidator<AtualizarUsuarioCommand>
    {
        public AtualizarUsuarioValidator()
        {
            RuleFor(x => x.UsuarioId)
                .NotEmpty().WithMessage("Id do usuário é obrigatório.");

            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório.")
                .MaximumLength(100).WithMessage("Nome não pode ter mais de 100 caracteres.");
        }
    }
}
