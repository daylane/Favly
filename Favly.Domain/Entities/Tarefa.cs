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
        public Guid? FamiliaId { get; private set; }          
        public Guid MembroDonoId { get; private set; }      
        public string Titulo { get; private set; }
        public string Descricao { get; private set; }
        public StatusTarefa Status { get; private set; }
        public EscopoTarefa Escopo { get; private set; }
        public DateTime ProximaOcorrencia { get; private set; }
        public RecorrenciaTarefa Recorrencia { get; private set; }

        public IReadOnlyCollection<Guid> MembrosAtribuidosIds => _membrosAtribuidosIds.AsReadOnly();

        protected Tarefa() { }
        private Tarefa(
          Guid? tarefaPaiId,
          Guid? familiaId,
          Guid membroDonoId,
          string titulo,
          string descricao,
          EscopoTarefa escopo,
          DateTime proximaOcorrencia,
          RecorrenciaTarefa recorrencia,
          List<Guid>? membrosAtribuidosIds)
        {
            Guard.AgainstEmptyGuid(membroDonoId, nameof(membroDonoId));
            Guard.AgainstNullOrWhiteSpace(titulo, nameof(titulo));
            Guard.AgainstInvalidEnum<EscopoTarefa>(escopo, nameof(escopo));

            // Tarefa de grupo exige FamiliaId
            if (escopo == EscopoTarefa.Grupo)
                Guard.Against<DomainException>(!familiaId.HasValue || familiaId == Guid.Empty,
                    "Tarefa de grupo requer um grupo válido.");

            // Tarefa pessoal com membros atribuídos deve ter ao menos um
            if (escopo == EscopoTarefa.Pessoal)
                Guard.Against<DomainException>(
                    membrosAtribuidosIds == null || !membrosAtribuidosIds.Any(),
                    "Tarefa pessoal precisa de ao menos um membro atribuído.");

            TarefaPaiId = tarefaPaiId;
            FamiliaId = familiaId;
            MembroDonoId = membroDonoId;
            Titulo = titulo;
            Descricao = descricao;
            Status = StatusTarefa.Pendente;
            Escopo = escopo;
            ProximaOcorrencia = proximaOcorrencia;
            Recorrencia = recorrencia;

            membrosAtribuidosIds?.ForEach(AdicionarMembro);
        }
        public static Tarefa CriarIndividual(
             Guid membroDonoId,
             string titulo,
             string descricao,
             DateTime proximaOcorrencia,
             RecorrenciaTarefa recorrencia)
        {
            return new Tarefa(
                tarefaPaiId: null,
                familiaId: null,
                membroDonoId: membroDonoId,
                titulo: titulo,
                descricao: descricao,
                escopo: EscopoTarefa.Pessoal,
                proximaOcorrencia: proximaOcorrencia,
                recorrencia: recorrencia,
                membrosAtribuidosIds: new List<Guid> { membroDonoId });
        }

        public static Tarefa CriarDoGrupo(
            Guid familiaId,
            Guid membroDonoId,
            string titulo,
            string descricao,
            DateTime proximaOcorrencia,
            RecorrenciaTarefa recorrencia,
            List<Guid>? membrosAtribuidos = null)
        {
            return new Tarefa(
                tarefaPaiId: null,
                familiaId: familiaId,
                membroDonoId: membroDonoId,
                titulo: titulo,
                descricao: descricao,
                escopo: EscopoTarefa.Grupo,
                proximaOcorrencia: proximaOcorrencia,
                recorrencia: recorrencia,
                membrosAtribuidosIds: membrosAtribuidos);
        }

        // --- Comportamentos ---

        public Tarefa CriarSubtarefa(string titulo, string descricao)
        {
            Guard.Against<DomainException>(
                Status == StatusTarefa.Concluido || Status == StatusTarefa.Cancelado,
                "Não é possível adicionar subtarefas a uma tarefa finalizada.");

            return new Tarefa(
                tarefaPaiId: Id,
                familiaId: FamiliaId,
                membroDonoId: MembroDonoId,
                titulo: titulo,
                descricao: descricao,
                escopo: Escopo,
                proximaOcorrencia: ProximaOcorrencia,
                recorrencia: Recorrencia,
                membrosAtribuidosIds: _membrosAtribuidosIds.ToList());
        }

        public void Concluir()
        {
            if (Status == StatusTarefa.Concluido) return;

            Status = StatusTarefa.Concluido;
            AtualizarDataAtualizacao();

            if (Recorrencia != null)
            {
                ProximaOcorrencia = Recorrencia.CalcularProximaData(ProximaOcorrencia);
                AddDomainEvent(new TarefaConcluidaEvent(this));
            }
        }

        public void Cancelar()
        {
            Guard.Against<DomainException>(
                Status == StatusTarefa.Concluido,
                "Não é possível cancelar uma tarefa já concluída.");

            Status = StatusTarefa.Cancelado;
            AtualizarAtivo();
            AtualizarDataAtualizacao();
        }

        public void AdicionarMembro(Guid membroId)
        {
            Guard.AgainstEmptyGuid(membroId, nameof(membroId));

            if (!_membrosAtribuidosIds.Contains(membroId))
                _membrosAtribuidosIds.Add(membroId);
        }

        public void RemoverMembro(Guid membroId)
        {
            Guard.Against<DomainException>(
                Escopo == EscopoTarefa.Pessoal && _membrosAtribuidosIds.Count <= 1,
                "Tarefa pessoal não pode ficar sem nenhum membro atribuído.");

            _membrosAtribuidosIds.Remove(membroId);
        }
    }
}


