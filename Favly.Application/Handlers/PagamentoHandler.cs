using Favly.Domain.Entities;
using Favly.Domain.Events;

namespace Favly.Application.Handlers
{
    public static class PagamentoHandler
    {
        public static async Task Handle(PagamentoRealizadoEvent @event)
        {
            var pago = @event.Pagamento;

            if (pago.Recorrencia == null) return;

            var proximaData = pago.Recorrencia.CalcularProximaData(pago.DataVencimento);

            var proximoPagamento = new Pagamento(
                pago.FamiliaId,
                pago.MembroResponsavelId,
                pago.Titulo,
                pago.Valor,
                proximaData,
                pago.Recorrencia
            );

           // await repository.AddAsync(proximoPagamento);
        }
    }
}
