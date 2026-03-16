using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Usuarios.Commands.DesativarUsuario
{
    public class DesativarUsuarioValidator : AbstractValidator<DesativarUsuarioCommand>
    {
        public DesativarUsuarioValidator()
        {
            RuleFor(x => x.UsuarioId)
                .NotEmpty().WithMessage("Id do usuário é obrigatório.");
        }

    }
}
