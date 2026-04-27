using Favly.Domain.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Domain.Events.Estoque
{
    public record EstoqueBaixoEvent(
         Guid ProdutoId,
         Guid GrupoId,
         string NomeProduto,
         decimal QuantidadeAtual,
         decimal QuantidadeMinima) : IDomainEvent;
}
