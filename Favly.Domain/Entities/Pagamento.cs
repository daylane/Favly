using Favly.Domain.Common.Base;
using Favly.Domain.Common.Enums;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Common.Validations;
using Favly.Domain.Events;
using Favly.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Favly.Domain.Entities
{
    public class Pagamento : Entity
    {
        public Guid? FamiliaId { get; private set; }             
        public Guid MembroResponsavelId { get; private set; }
        public Guid? TarefaId { get; private set; }               
        public string Titulo { get; private set; }
        public DinheiroPagamento Valor { get; private set; }
        public RecorrenciaPagamento Recorrencia { get; private set; }
        public DateTime DataVencimento { get; private set; }
        public DateTime? DataPagamento { get; private set; }     
        public StatusPagamento Status { get; private set; }
        public EscopoPagamento Escopo { get; private set; }

        protected Pagamento() { }

        private Pagamento(
            Guid? familiaId,
            Guid membroResponsavelId,
            Guid? tarefaId,
            string titulo,
            DinheiroPagamento valor,
            DateTime dataVencimento,
            RecorrenciaPagamento recorrencia,
            EscopoPagamento escopo)
        {
            Guard.AgainstEmptyGuid(membroResponsavelId, nameof(membroResponsavelId));
            Guard.AgainstNullOrWhiteSpace(titulo, nameof(titulo));
            Guard.AgainstNull(valor, nameof(valor));
            Guard.AgainstInvalidEnum<EscopoPagamento>(escopo, nameof(escopo));

            if (escopo == EscopoPagamento.Grupo)
                Guard.Against<DomainException>(!familiaId.HasValue || familiaId == Guid.Empty,
                    "Despesa de grupo requer um grupo válido.");

            FamiliaId = familiaId;
            MembroResponsavelId = membroResponsavelId;
            TarefaId = tarefaId;
            Titulo = titulo;
            Valor = valor;
            DataVencimento = dataVencimento;
            Recorrencia = recorrencia;
            Escopo = escopo;
            Status = StatusPagamento.Pendente;
        }

        public static Pagamento CriarDespesaDoGrupo(
             Guid familiaId,
             Guid membroResponsavelId,
             string titulo,
             DinheiroPagamento valor,
             RecorrenciaPagamento recorrencia,
             Guid? tarefaId = null)
        {
            var dataVencimento = recorrencia.CalcularProximoVencimento(DateTime.UtcNow);

            return new Pagamento(
                familiaId: familiaId,
                membroResponsavelId: membroResponsavelId,
                tarefaId: tarefaId,
                titulo: titulo,
                valor: valor,
                dataVencimento: dataVencimento,
                recorrencia: recorrencia,
                escopo: EscopoPagamento.Grupo);
        }

        public static Pagamento CriarDespesaIndividual(
            Guid membroResponsavelId,
            string titulo,
            DinheiroPagamento valor,
            RecorrenciaPagamento recorrencia,
            Guid? tarefaId = null)
        {
            var dataVencimento = recorrencia.CalcularProximoVencimento(DateTime.UtcNow);

            return new Pagamento(
                familiaId: null,
                membroResponsavelId: membroResponsavelId,
                tarefaId: tarefaId,
                titulo: titulo,
                valor: valor,
                dataVencimento: dataVencimento,
                recorrencia: recorrencia,
                escopo: EscopoPagamento.Individual);
        }

        // --- Comportamentos ---

        public void Pagar()
        {
            Guard.Against<DomainException>(Status == StatusPagamento.Pago,
                "Este pagamento já foi realizado.");

            Status = StatusPagamento.Pago;
            DataPagamento = DateTime.UtcNow;

            // Calcula próximo vencimento e dispara evento para criar próxima ocorrência
            if (Recorrencia != null)
            {
                DataVencimento = Recorrencia.CalcularProximoVencimento(DataVencimento);
                AddDomainEvent(new PagamentoRealizadoEvent(this));
            }

            AtualizarDataAtualizacao();
        }

        public void Cancelar()
        {
            Guard.Against<DomainException>(Status == StatusPagamento.Pago,
                "Não é possível cancelar um pagamento já realizado.");

            Status = StatusPagamento.Cancelado;
            AtualizarAtivo();
            AtualizarDataAtualizacao();
        }

        public void VerificarAtraso()
        {
            if (Status == StatusPagamento.Pendente && DateTime.UtcNow.Date > DataVencimento.Date)
            {
                Status = StatusPagamento.Atrasado;
                AtualizarDataAtualizacao();
            }
        }

    }
}
