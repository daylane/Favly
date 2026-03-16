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
        public FrequenciaOcorrencia Frequencia { get; private set; }

        protected RecorrenciaPagamento() { } // EF Core

        private RecorrenciaPagamento(int diaVencimento, FrequenciaOcorrencia frequencia)
        {
            Guard.AgainstInvalidEnum<FrequenciaOcorrencia>(frequencia, nameof(frequencia));
            Guard.Against<DomainException>(diaVencimento < 1 || diaVencimento > 31, "Dia de vencimento deve estar entre 1 e 31.");

            DiaVencimento = diaVencimento;
            Frequencia = frequencia;
        }
        public static RecorrenciaPagamento Criar(int diaVencimento, FrequenciaOcorrencia frequencia)
           => new(diaVencimento, frequencia);

        // Atalhos semânticos
        public static RecorrenciaPagamento CriarMensal(int diaVencimento)
            => new(diaVencimento, FrequenciaOcorrencia.Mensal);

        public static RecorrenciaPagamento CriarAnual(int diaVencimento)
            => new(diaVencimento, FrequenciaOcorrencia.Anual);

        public DateTime CalcularProximoVencimento(DateTime dataAtual)
        {
            return Frequencia switch
            {
                FrequenciaOcorrencia.Mensal => new DateTime(dataAtual.Year, dataAtual.Month, DiaVencimento)
                                                .AddMonths(dataAtual.Day >= DiaVencimento ? 1 : 0),
                FrequenciaOcorrencia.Anual => new DateTime(dataAtual.Year + 1, dataAtual.Month, DiaVencimento),
                _ => throw new DomainException($"Frequência '{Frequencia}' não suportada.")
            };
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return DiaVencimento;
            yield return Frequencia;
        }

    }
}
