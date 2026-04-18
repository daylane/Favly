using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Movimentacoes.DTOs
{
    public record RegistrarSaidaRequest(Guid ProdutoId, Guid MembroId, decimal Quantidade, string? Observacao);
}
