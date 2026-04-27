using Favly.Domain.Common.Base;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Common.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Domain.Entities
{
    public class TokenResetSenha : Entity
    {
        public Guid UsuarioId { get; private set; }
        public string Token { get; private set; }
        public DateTime Expiracao { get; private set; }
        public bool Usado { get; private set; }

        protected TokenResetSenha() { }

        private TokenResetSenha(Guid usuarioId, string token, DateTime expiracao)
        {
            UsuarioId = usuarioId;
            Token = token;
            Expiracao = expiracao;
            Usado = false;
        }

        public static TokenResetSenha Criar(Guid usuarioId)
        {
            Guard.AgainstEmptyGuid(usuarioId, nameof(usuarioId));

            var token = Guid.NewGuid().ToString("N"); // 32 caracteres
            var expiracao = DateTime.UtcNow.AddHours(1);

            return new TokenResetSenha(usuarioId, token, expiracao);
        }

        public void Validar()
        {
            DomainException.When(Usado, "Este token já foi utilizado.");
            DomainException.When(DateTime.UtcNow > Expiracao, "Token expirado. Solicite um novo link de recuperação.");
        }

        public void Usar()
        {
            Validar();
            Usado = true;
            AtualizarDataAtualizacao();
        }
    }
}

