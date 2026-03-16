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
        public FrequenciaOcorrencia Frequencia { get; private set; } 
        public int Intervalo { get; private set; }

        protected RecorrenciaTarefa() { } // EF Core


        private RecorrenciaTarefa(List<DiasDaSemana> diasDaSemana, FrequenciaOcorrencia frequencia, int intervalo)
        {
            Guard.AgainstNull(diasDaSemana, nameof(diasDaSemana));
            Guard.AgainstInvalidEnum<FrequenciaOcorrencia>(frequencia, nameof(frequencia));
            Guard.Against<DomainException>(intervalo <= 0, "Intervalo deve ser maior que zero.");
            Guard.Against<DomainException>(frequencia != FrequenciaOcorrencia.Diaria && !diasDaSemana.Any(), "Informe ao menos um dia da semana para recorrência semanal.");


            Intervalo = intervalo;
            DiasDaSemana = diasDaSemana;
            Frequencia = frequencia;
        }

        public static RecorrenciaTarefa Criar(List<DiasDaSemana> diasDaSemana, FrequenciaOcorrencia frequencia, int intervalo)
              => new(diasDaSemana, frequencia, intervalo);

        public static RecorrenciaTarefa CriarDiaria()
            => new(new List<DiasDaSemana>(), FrequenciaOcorrencia.Diaria, 1);

        public static RecorrenciaTarefa CriarSemanal(List<DiasDaSemana> dias, int intervalo = 1)
            => new(dias, FrequenciaOcorrencia.Semanal, intervalo);

        public static RecorrenciaTarefa CriarMensal(int intervalo = 1)
            => new(new List<DiasDaSemana>(), FrequenciaOcorrencia.Mensal, intervalo);

        public DateTime CalcularProximaData(DateTime dataAtual)
        {
            return Frequencia switch
            {
                FrequenciaOcorrencia.Diaria => dataAtual.AddDays(Intervalo),         
                FrequenciaOcorrencia.Semanal => CalcularProximoDiaDaSemana(dataAtual),
                FrequenciaOcorrencia.Mensal => dataAtual.AddMonths(Intervalo),
                _ => throw new DomainException($"Frequência '{Frequencia}' não suportada.")
            };
        }

        private DateTime CalcularProximoDiaDaSemana(DateTime dataAtual)
        {
            if (!DiasDaSemana.Any())
                return dataAtual.AddDays(7 * Intervalo);

            var diasOrdenados = DiasDaSemana
                .Select(d => (int)d)
                .OrderBy(d => d)
                .ToList();

            var diaAtual = (int)dataAtual.DayOfWeek;
            var proximoDia = diasOrdenados.FirstOrDefault(d => d > diaAtual);

            if (proximoDia == 0) 
            {
                var diasAteProximaSemana = 7 - diaAtual + diasOrdenados.First();
                return dataAtual.AddDays(diasAteProximaSemana + (7 * (Intervalo - 1)));
            }

            return dataAtual.AddDays(proximoDia - diaAtual);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            foreach (var dia in DiasDaSemana.OrderBy(d => d))
                yield return dia;

            yield return Frequencia;
            yield return Intervalo;
        }
    }
}

