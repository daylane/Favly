using Favly.Domain.Common.Base;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Common.Validations;
using Favly.Domain.Events;
using Favly.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Favly.Domain.Entities
{
    public class Usuario : Entity
    {
        public EmailUsuario Email { get; private set; }
        public string Nome { get; private set; }
        public string Hash { get; private set; }
        public string Avatar { get; private set; }
        public string CodigoAtivacao { get; private set; }

        protected Usuario() { } // EF Core

        private Usuario(EmailUsuario email, string nome, string hash, string avatar, string codigoAtivacao)
        {
            Email = email;
            Nome = nome;
            Hash = hash;
            Avatar = avatar;
            CodigoAtivacao = codigoAtivacao;
            DataAtualizacao = DataCriacao;

            AddDomainEvent(new UsuarioCriadoEvent(Id, email.EnderecoEmail, nome));
        }

        public static Usuario Criar(string email, string nome, string hash, string avatar)
        {
            Guard.AgainstNullOrWhiteSpace(nome, nameof(nome));
            Guard.AgainstNullOrWhiteSpace(hash, nameof(hash));
            Guard.Against<DomainException>(nome.Length > 100, "Nome não pode ter mais de 100 caracteres.");

            var emailVO = EmailUsuario.Criar(email);
            var codigoAtivacao = Guid.NewGuid().ToString("N")[..8].ToUpper();

            return new Usuario(emailVO, nome.Trim(), hash, avatar ?? string.Empty, codigoAtivacao);
        }

        public void Ativar(string codigo)
        {
            Guard.Against<DomainException>(Ativo, "Usuário já está ativo.");
            Guard.Against<DomainException>(!CodigoAtivacao.Equals(codigo, StringComparison.OrdinalIgnoreCase), "Código de ativação inválido.");

            AtualizarAtivo();
            CodigoAtivacao = string.Empty;
            AtualizarDataAtualizacao();

            AddDomainEvent(new UsuarioAtivatoEvent(Id));
        }

        public void Atualizar(string nome, string avatar)
        {
            Guard.Against<DomainException>(!Ativo, "Não é possível atualizar um usuário inativo.");
            Guard.AgainstNullOrWhiteSpace(nome, nameof(nome));
            Guard.Against<DomainException>(nome.Length > 100, "Nome não pode ter mais de 100 caracteres.");

            Nome = nome.Trim();
            Avatar = avatar ?? Avatar;
            AtualizarDataAtualizacao();

            AddDomainEvent(new UsuarioAtualizadoEvent(Id, Nome, Avatar));
        }

        public void Desativar()
        {
            Guard.Against<DomainException>(!Ativo, "Usuário já está inativo.");

            AtualizarAtivo();
            AtualizarDataAtualizacao();

            AddDomainEvent(new UsuarioDesativadoEvent(Id));
        }

        public bool ValidarSenha(string hash) => Hash == hash;
    }
}