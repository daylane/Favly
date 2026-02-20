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
        public TipoRecorrencia TipoRecorrencia { get; private set; } 
        public int Intervalo { get; private set; }
    
        private RecorrenciaTarefa(TipoRecorrencia recorrencia, int intervalo)
        {
            Guard.AgainstNull(recorrencia, nameof(recorrencia));
            Guard.AgainstNull(intervalo, nameof(intervalo));
            Guard.AgainstInvalidEnum<TipoRecorrencia>(recorrencia, nameof(recorrencia));

            if (intervalo < 0)
                throw new DomainException("Intervalo deve ser maior que zero.");

            Intervalo = intervalo;
            TipoRecorrencia = recorrencia;
        }

        public static RecorrenciaTarefa Criar(TipoRecorrencia recorrencia, int intervalo)
        {
            return new RecorrenciaTarefa(recorrencia, intervalo);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return TipoRecorrencia;
            yield return Intervalo;
        }
    }
}
