using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Produtos.Commands.RemoverProduto
{
    public record RemoverProdutoCommand(Guid ProdutoId);

}
