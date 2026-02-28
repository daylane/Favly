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
        public Guid FamiliaId { get; private set; }
        public Guid MembroResponsavelId { get; private set; }
        public Guid TarefaId { get; private set; }
        public string Titulo { get; private set; }
        public DinheiroPagamento Valor { get; private set; }
        public RecorrenciaTarefa Recorrencia { get; private set; }
        public DateTime DataVencimento { get; private set; }
        public DateTime DataPagamento { get; private set; }
        public StatusPagamento Status { get; private set; }

        protected Pagamento() { }

        public Pagamento(
            Guid familiaId,
            Guid membroResponsavelId,
            string titulo,
            DinheiroPagamento valor,
            DateTime dataVencimento,
            RecorrenciaTarefa recorrencia)
        {
            Guard.AgainstEmptyGuid(familiaId, nameof(familiaId));
            Guard.AgainstEmptyGuid(membroResponsavelId, nameof(membroResponsavelId));
            Guard.AgainstNullOrWhiteSpace(titulo, nameof(titulo));
            Guard.AgainstNull(valor, nameof(valor));

            FamiliaId = familiaId;
            MembroResponsavelId = membroResponsavelId;
            Titulo = titulo;
            Valor = valor;
            DataVencimento = dataVencimento;
            Recorrencia = recorrencia;
            Status = StatusPagamento.Pendente;
        }

        public void Pagar()
        {
            if (Status == StatusPagamento.Pago) return;

            Status = StatusPagamento.Pago;
            DataAtualizacao = DateTime.UtcNow;

            if (Recorrencia != null)
                AddDomainEvent(new PagamentoRealizadoEvent(this));
        }

        public void Cancelar()
        {
            Guard.Against<DomainException>(Status == StatusPagamento.Pago, "Não é possível cancelar um pagamento que já foi realizado.");

            Status = StatusPagamento.Cancelado;
            Ativo = false;
            DataAtualizacao = DateTime.UtcNow;
        }

        public void VerificarAtraso()
        {
            if (Status == StatusPagamento.Pendente && DateTime.UtcNow.Date > DataVencimento.Date)
            {
                Status = StatusPagamento.Atrasado;
            }
        }

    }
}
