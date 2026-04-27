using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Movimentacoes.DTOs
{
    public record RegistrarEntradaRequest(Guid ProdutoId, decimal Quantidade, decimal? Preco, Guid? MercadoId, string? Observacao);
}
