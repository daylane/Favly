using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Usuarios.Commands.AtivarUsuario
{
    public class AtivarUsuarioValidator : AbstractValidator<AtivarUsuarioCommand>
    {
        public AtivarUsuarioValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email do usuário é obrigatório.");

            RuleFor(x => x.CodigoAtivacao)
                .NotEmpty().WithMessage("Código de ativação é obrigatório.")
                .Length(8).WithMessage("Código de ativação inválido.");
        }
    }
}
