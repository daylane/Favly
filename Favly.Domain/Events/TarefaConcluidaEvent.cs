using Favly.Domain.Common.Base;
using Favly.Domain.Common.Enums;
using Favly.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Domain.Events
{
    public record TarefaConcluidaEvent(Tarefa Tarefa) : DomainEventBase;
}
