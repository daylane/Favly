using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Movimentacoes.Commands.RegistrarEntrada
{
    public class RegistrarEntradaValidator : AbstractValidator<RegistrarEntradaCommand>
    {
        public RegistrarEntradaValidator()
        {
            RuleFor(x => x.GrupoId).NotEmpty();
            RuleFor(x => x.ProdutoId).NotEmpty();
            RuleFor(x => x.MembroId).NotEmpty();
            RuleFor(x => x.Quantidade).GreaterThan(0);
            RuleFor(x => x.Preco).GreaterThanOrEqualTo(0).When(x => x.Preco.HasValue);
        }
    }
}
