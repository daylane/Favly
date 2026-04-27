using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Produtos.Commands.CriarProduto
{
    public class CriarProdutoValidator : AbstractValidator<CriarProdutoCommand>
    {
        public CriarProdutoValidator()
        {
            RuleFor(x => x.GrupoId).NotEmpty();
            RuleFor(x => x.CategoriaId).NotEmpty();
            RuleFor(x => x.Nome).NotEmpty().MaximumLength(100);
            RuleFor(x => x.QuantidadeMinima).GreaterThanOrEqualTo(0);
        }
    }
}
