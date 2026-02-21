using Favly.Domain.Common.Base;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Common.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Domain.ValueObjects
{
    public class DinheiroPagamento : ValueObject
    {
        public decimal Valor { get; private set; }
        public string Moeda { get; private set; } // Ex: "BRL", "USD"

        private DinheiroPagamento(decimal valor, string moeda)
        {
            Guard.AgainstNull(valor, nameof(valor));
            Guard.AgainstNullOrWhiteSpace(moeda, nameof(moeda));

            if (valor < 0) 
                throw new DomainException("O valor não pode ser negativo.");

            Valor = valor;
            Moeda = moeda.ToUpper();
        }

        public static DinheiroPagamento Criar(decimal valor, string moeda)
        {
            return new DinheiroPagamento(valor, moeda);
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Valor;
            yield return Moeda;
        }
    }
}
