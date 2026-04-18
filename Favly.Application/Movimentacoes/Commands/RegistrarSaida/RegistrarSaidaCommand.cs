using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Movimentacoes.Commands.RegistrarSaida
{
    public record RegistrarSaidaCommand(
        Guid GrupoId,
        Guid ProdutoId,
        Guid MembroId,
        decimal Quantidade,
        string? Observacao = null);
}
