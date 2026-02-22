using Favly.Domain.Common.Base;
using Favly.Domain.Common.Enums;
using Favly.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Domain.Entities
{
    public class NotificacaoPagamento : Entity
    {
        public Guid FamiliaId { get; private set; }
        public Guid MembroResponsavelId { get; private set; } 
        public string Titulo { get; private set; }

        public DinheiroPagamento Valor { get; private set; }
        public RecorrenciaTarefa Recorrencia { get; private set; }

        public DateTime DataVencimento { get; private set; }
        public StatusPagamento Status { get; private set; }
    }
}
