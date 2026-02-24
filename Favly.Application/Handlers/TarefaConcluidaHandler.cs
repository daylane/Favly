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

            if (antiga.Recorrencia == null) return;

            var proximaData = antiga.Recorrencia.CalcularProximaData(antiga.ProximaOcorrencia);

            var novaTarefa = new Tarefa(
                antiga.TarefaPaiId,
                antiga.MembrosAtribuidosIds.ToList(), 
                antiga.FamiliaId,
                antiga.Titulo,
                antiga.Descricao,
                StatusTarefa.Pendente,
                antiga.Escopo,
                proximaData,
                antiga.Recorrencia
            );

            // Como o construtor já adicionou os membros, você não precisa mais do foreach aqui! (DRY)

           // await _repository.AddAsync(novaTarefa);
        }
    }
}

