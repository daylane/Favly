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
        public FrequenciaTarefa Frequencia { get; private set; } // "semanal"
        public int Intervalo { get; private set; }

        private RecorrenciaTarefa(List<DiasDaSemana> diasDaSemana, FrequenciaTarefa frequencia, int intervalo)
        {
            Guard.AgainstNull(diasDaSemana, nameof(diasDaSemana));
            Guard.AgainstNull(intervalo, nameof(intervalo));
            Guard.AgainstInvalidEnum<FrequenciaTarefa>(frequencia, nameof(frequencia));
            Guard.AgainstInvalidEnum<DiasDaSemana>(diasDaSemana, nameof(diasDaSemana));

            if (intervalo <= 0)
                throw new DomainException("Intervalo não pode ser negativo");

            DiasDaSemana = diasDaSemana;
            Frequencia = frequencia;
        }

        public static RecorrenciaTarefa Criar(List<DiasDaSemana> recorrencia, FrequenciaTarefa frequencia, int intervalo)
        {
            return new RecorrenciaTarefa(recorrencia, frequencia, intervalo);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return DiasDaSemana; //foreach (var dia in DiasDaSemana.OrderBy(d => d)) yield return dia;
            yield return Frequencia;
        }
    }
}
