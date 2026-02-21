using Favly.Domain.Common.Base;
using Favly.Domain.Common.Enums;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Common.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Domain.ValueObjects
{
    public class RecorrenciaPagamento : ValueObject
    {
        public int DiaVencimento { get; }
        public string Frequencia { get; } // "Mensal"

        public RecorrenciaPagamento(int diaVencimento, string frequencia)
        {
            Guard.AgainstNull(diaVencimento, nameof(diaVencimento));
            Guard.AgainstNullOrWhiteSpace(frequencia, nameof(frequencia));

            if (diaVencimento < 1 || diaVencimento > 31)
                throw new DomainException("Dia de vencimento inválido.");

            DiaVencimento = diaVencimento;
            Frequencia = frequencia;
        }
        public static RecorrenciaPagamento Criar(int diaVencimento, string frequencia)
        {
            return new RecorrenciaPagamento(diaVencimento, frequencia);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return DiaVencimento;
            yield return Frequencia;
        }

    }
}
