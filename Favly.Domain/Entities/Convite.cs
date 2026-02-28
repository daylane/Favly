using Favly.Domain.Common.Base;
using Favly.Domain.Common.Enums;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Common.Validations;
using Favly.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Favly.Domain.Entities
{
    public  class Convite : Entity
    {
        public Guid FamiliaId { get; private set; }
        public EmailUsuario EmailConvidado { get; private set; }
        public string Codigo { get; private set; } // Token único para o link de convite
        public StatusConvite Status { get; private set; }
        public DateTime DataExpiracao { get; private set; }

        protected Convite() { } // EF Core

        public Convite(Guid familiaId, EmailUsuario emailConvidado)
        {
            Guard.AgainstEmptyGuid(familiaId, nameof(familiaId));
            Guard.AgainstNull(emailConvidado, nameof(emailConvidado));

            FamiliaId = familiaId;
            EmailConvidado = emailConvidado;
            Status = StatusConvite.Pendente;

            DataExpiracao = DateTime.UtcNow.AddDays(7);

            Codigo = Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper();
        }

        // --- Comportamentos (DDD) ---

        public void Aceitar()
        {
            ValidarSePodeMudarEstado();
            Status = StatusConvite.Aceito;
            DataAtualizacao = DateTime.UtcNow;
        }

        public void Recusar()
        {
            ValidarSePodeMudarEstado();
            Status = StatusConvite.Recusado;
            DataAtualizacao = DateTime.UtcNow;
        }

        public void Expirar()
        {
            if (Status == StatusConvite.Pendente && DateTime.UtcNow > DataExpiracao)
            {
                Status = StatusConvite.Expirado;
                DataAtualizacao = DateTime.UtcNow;
            }
        }

        private void ValidarSePodeMudarEstado()
        {
            Guard.Against<DomainException>(Status != StatusConvite.Pendente, "Este convite já foi processado ou está expirado.");

            if (DateTime.UtcNow > DataExpiracao)
            {
                Status = StatusConvite.Expirado;
                throw new DomainException("O prazo deste convite expirou.");
            }
        }
    }
}
