using Favly.Domain.Common.Base;
using Favly.Domain.Common.Enums;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Common.Validations;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Favly.Domain.Entities
{
    public class Grupo : AggregateRoot
    {
        public string Nome { get; private set; }
        public string Convite { get; private set; }
        public string Avatar { get; private set; }

        private readonly List<Membro> _membros = new();
        public IReadOnlyCollection<Membro> Membros => _membros.AsReadOnly();

        protected Grupo() { }

        public Grupo(string nome, string avatar = null)
        {
            Guard.AgainstNullOrWhiteSpace(nome, nameof(nome));

            Nome = nome;
            Avatar = avatar ?? "default-avatar.png";
            Convite = GerarCodigoConvite(); 
        }

        public void AdicionarMembro(Guid usuarioId, string apelido, PapelMembro role)
        {
            Guard.Against<DomainException>(_membros.Any(m => m.UsuarioId == usuarioId), "Este usuário já faz parte deste grupo.");

            var novoMembro = new Membro(this.Id, usuarioId, apelido, role);
            _membros.Add(novoMembro);
        }

        public void AlterarAvatar(string novaUrl)
        {
            Guard.AgainstNullOrWhiteSpace(novaUrl, nameof(novaUrl));
            Avatar = novaUrl;
            DataAtualizacao = DateTime.UtcNow;
        }

        public void RedefinirConvite()
        {
            Convite = GerarCodigoConvite();
            DataAtualizacao = DateTime.UtcNow;
        }

        private string GerarCodigoConvite()
        {
            return Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        }

    }
}