using Favly.Domain.Common.Base;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Common.Validations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Favly.Domain.ValueObjects
{
    public class EmailUsuario : ValueObject
    {
        private static readonly Regex EmailRegex = new Regex(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public string EnderecoEmail { get; private set; }

        private EmailUsuario(string enderecoEmail)
        {
            Guard.AgainstNullOrWhiteSpace(enderecoEmail, nameof(enderecoEmail));

            var emailLimpo = enderecoEmail.Trim().ToLower();

            if (!EmailRegex.IsMatch(emailLimpo))
                throw new DomainException("O formato do e-mail é inválido.");

            if (emailLimpo.EndsWith("@teste.com") || emailLimpo.EndsWith("@example.com"))
                throw new DomainException("E-mails de teste não são permitidos.");

            EnderecoEmail = emailLimpo;
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
