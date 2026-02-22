using Favly.Domain.Common.Base;
using Favly.Domain.Common.Enums;
using Favly.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Favly.Domain.Entities
{
    public class Tarefa : Entity
    {
        private readonly List<Guid> _membrosAtribuidosIds = new();

        public Guid? TarefaPaiId { get; private set; }
        public Guid FamiliaId { get; private set; }
        public string Titulo { get; private set; }
        public string Descricao { get; private set; }
        public StatusTarefa Status { get; private set; }
        public EscopoTarefa Escopo { get; private set; }
        public DateTime ProximaOcorrencia { get; private set; }
        public RecorrenciaTarefa Recorrencia { get; private set; }

        public IReadOnlyCollection<Guid> MembrosAtribuidosIds => _membrosAtribuidosIds.AsReadOnly();
    }
}


