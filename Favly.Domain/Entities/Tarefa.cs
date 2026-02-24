using Favly.Domain.Common.Base;
using Favly.Domain.Common.Enums;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Common.Validations;
using Favly.Domain.Events;
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

        public Tarefa(Guid? tarefaPaiId, List<Guid>? membrosAtribuidosIds, Guid familiaId, string titulo, string descricao, StatusTarefa status, EscopoTarefa escopo, DateTime proximaOcorrencia, RecorrenciaTarefa recorrencia)
        {
            Guard.AgainstEmptyGuid(familiaId, nameof(familiaId));
            Guard.AgainstNullOrWhiteSpace(titulo, nameof(titulo));
            Guard.AgainstInvalidEnum<StatusTarefa>(status, nameof(status));
            Guard.AgainstInvalidEnum<EscopoTarefa>(escopo, nameof(escopo));

            if (escopo == EscopoTarefa.Pessoal)
                Guard.Against<DomainException>(
                    membrosAtribuidosIds == null || !membrosAtribuidosIds.Any(),
                    "Uma tarefa de escopo Pessoal precisa de pelo menos um membro atribuído.");


            TarefaPaiId = tarefaPaiId;
            FamiliaId = familiaId;
            Titulo = titulo;
            Descricao = descricao;
            Status = status;
            Escopo = escopo;
            ProximaOcorrencia = proximaOcorrencia;
            Recorrencia = recorrencia;

            membrosAtribuidosIds?.ForEach(id => { AdicionarMembro(id); });
               
        }
        public Tarefa CriarSubtarefa(string titulo, string descricao)
        {
            Guard.Against<DomainException>(
                Status == StatusTarefa.Concluido || Status == StatusTarefa.Cancelado,
                "Não é possível adicionar subtarefas a uma tarefa finalizada.");

            var subtarefa = new Tarefa(
                tarefaPaiId: this.Id,
                membrosAtribuidosIds: this._membrosAtribuidosIds, 
                familiaId: this.FamiliaId,
                titulo: titulo,
                descricao: descricao,
                status: StatusTarefa.Pendente,
                escopo: this.Escopo, 
                proximaOcorrencia: this.ProximaOcorrencia,
                recorrencia: this.Recorrencia
            );

            return subtarefa;
        }

        public void Concluir()
        {
            if (Status == StatusTarefa.Concluido) return;

            Status = StatusTarefa.Concluido;
            DataAtualizacao = DateTime.UtcNow;

            if (Recorrencia != null)
                AddDomainEvent(new TarefaConcluidaEvent(this));
        }
        public void Cancelar()
        {
            Status = StatusTarefa.Cancelado;
            Ativo = false;
            DataAtualizacao = DateTime.UtcNow;
        }

        public void AdicionarMembro(Guid membroId)
        {
            Guard.AgainstEmptyGuid(membroId, nameof(membroId));

            if (!_membrosAtribuidosIds.Contains(membroId))
                _membrosAtribuidosIds.Add(membroId);
        }

        public void RemoverMembro(Guid membroId)
        {
            Guard.Against<DomainException>(Escopo == EscopoTarefa.Pessoal && _membrosAtribuidosIds.Count <= 1, "Uma tarefa pessoal não pode ficar sem nenhum membro atribuído.");

            _membrosAtribuidosIds.Remove(membroId);
        }
    }
}


