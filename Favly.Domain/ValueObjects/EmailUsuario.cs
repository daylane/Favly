using Favly.Domain.Common.Base;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Common.Validations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Favly.Domain.ValueObjects
{
    public class EmailUsuario : ValueObject
    {
        public string EnderecoEmail { get; private set; }

        private EmailUsuario(string enderecoEmail)
        {
            Guard.AgainstNullOrWhiteSpace(enderecoEmail, nameof(enderecoEmail));

            if (!enderecoEmail.Contains("@"))
                throw new DomainException("Formato de e-mail inválido.");

            EnderecoEmail = enderecoEmail.Trim().ToLower();
        }

        public static EmailUsuario Criar(string enderecoEmail)
        {
            return new EmailUsuario(enderecoEmail);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return EnderecoEmail;
        }
    }
}
