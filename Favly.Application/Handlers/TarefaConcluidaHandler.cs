using Favly.Domain.Common.Enums;
using Favly.Domain.Entities;
using Favly.Domain.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Handlers
{
    public class TarefaConcluidaHandler : INotificationHandler<TarefaConcluidaEvent>
    {
        public async Task Handle(TarefaConcluidaEvent notification, CancellationToken cancellationToken)
        {
            var antiga = notification.Tarefa;

            // Se não for recorrente, não fazemos nada
            if (antiga.Recorrencia == null) return;

            // Criamos a nova instância para o futuro
            var proximaData = antiga.Recorrencia.CalcularProximaData(antiga.ProximaOcorrencia);

            var novaTarefa = new Tarefa(
                antiga.TarefaPaiId,
                antiga.FamiliaId,
                antiga.Titulo,
                antiga.Descricao,
                StatusTarefa.Pendente,
                antiga.Escopo,
                proximaData,
                antiga.Recorrencia
            );

            foreach (var membroId in antiga.MembrosAtribuidosIds)
            {
                novaTarefa.AdicionarMembro(membroId);
            }

           //await _repository.AddAsync(novaTarefa);
            // O Unit of Work salvará isso no banco ao final da transação
        }
    }
}
