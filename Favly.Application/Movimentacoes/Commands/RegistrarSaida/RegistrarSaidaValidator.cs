using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Movimentacoes.Commands.RegistrarSaida
{
    public class RegistrarSaidaValidator : AbstractValidator<RegistrarSaidaCommand>
    {
        public RegistrarSaidaValidator()
        {
            RuleFor(x => x.GrupoId).NotEmpty();
            RuleFor(x => x.ProdutoId).NotEmpty();
            RuleFor(x => x.MembroId).NotEmpty();
            RuleFor(x => x.Quantidade).GreaterThan(0);
        }
    }
}
