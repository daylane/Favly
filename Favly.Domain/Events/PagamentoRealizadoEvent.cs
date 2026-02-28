using Favly.Domain.Common.Base;
using Favly.Domain.Entities;
using Favly.Domain.ValueObjects;

namespace Favly.Domain.Events
{
    public record PagamentoRealizadoEvent(Pagamento pagamento
                                          ) : DomainEventBase;
}
