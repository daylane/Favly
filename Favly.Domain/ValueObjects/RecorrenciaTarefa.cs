using Favly.Domain.Common.Base;
using Favly.Domain.Common.Enums;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Common.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Domain.ValueObjects
{
    public class RecorrenciaTarefa : ValueObject
    {
        public List<DiasDaSemana> DiasDaSemana { get; private set; }
        public string Frequencia { get; private set; } // "semanal"

        private RecorrenciaTarefa(List<DiasDaSemana> diasDaSemana, string frequencia)
        {
            Guard.AgainstNullOrWhiteSpace(frequencia, nameof(frequencia));
            Guard.AgainstNull(diasDaSemana, nameof(diasDaSemana));
            Guard.AgainstInvalidEnum<DiasDaSemana>(diasDaSemana, nameof(diasDaSemana));


            DiasDaSemana = diasDaSemana;
            Frequencia = frequencia;
        }

        public static RecorrenciaTarefa Criar(List<DiasDaSemana> recorrencia, string frequencia)
        {
            return new RecorrenciaTarefa(recorrencia, frequencia);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return DiasDaSemana; //foreach (var dia in DiasDaSemana.OrderBy(d => d)) yield return dia;
            yield return Frequencia;
        }
    }
}
