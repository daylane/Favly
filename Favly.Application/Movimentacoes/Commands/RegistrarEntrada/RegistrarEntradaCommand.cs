using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Movimentacoes.Commands.RegistrarEntrada
{
    public record RegistrarEntradaCommand(
       Guid GrupoId,
       Guid ProdutoId,
       Guid MembroId,
       decimal Quantidade,
       decimal? Preco = null,
       Guid? MercadoId = null,
       string? Observacao = null);
}
