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

        public void AlterarNome(string novoNome)
        {
            Guard.AgainstNullOrWhiteSpace(novoNome, nameof(novoNome));
            Nome = novoNome;
            DataAtualizacao = DateTime.UtcNow;
        }

        public void AlterarAvatar(string novaUrl)
        {
            Guard.AgainstNullOrWhiteSpace(novaUrl, nameof(novaUrl));
            Avatar = novaUrl;
            DataAtualizacao = DateTime.UtcNow;
        }

        public Membro ExpulsarMembro(Guid adminId, Guid membroId)
        {
            var admin = _membros.FirstOrDefault(m => m.UsuarioId == adminId);
            Guard.Against<DomainException>(
                admin is null || admin.Role != PapelMembro.Administrador,
                "Apenas administradores podem remover membros.");

            var alvo = _membros.FirstOrDefault(m => m.Id == membroId);
            Guard.Against<DomainException>(alvo is null, "Membro não encontrado.");
            Guard.Against<DomainException>(
                alvo!.UsuarioId == adminId,
                "Use o endpoint 'sair' para sair do grupo.");

            _membros.Remove(alvo);
            DataAtualizacao = DateTime.UtcNow;

            return alvo;
        }

        public void AlterarPapelMembro(Guid adminId, Guid membroId, PapelMembro novoPapel)
        {
            var admin = _membros.FirstOrDefault(m => m.UsuarioId == adminId);
            Guard.Against<DomainException>(
                admin is null || admin.Role != PapelMembro.Administrador,
                "Apenas administradores podem alterar papéis.");

            var alvo = _membros.FirstOrDefault(m => m.Id == membroId);
            Guard.Against<DomainException>(alvo is null, "Membro não encontrado.");
            Guard.Against<DomainException>(
                alvo!.UsuarioId == adminId,
                "Você não pode alterar seu próprio papel.");

            alvo.MudarPapel(novoPapel);
            DataAtualizacao = DateTime.UtcNow;
        }

        public Membro SairDoGrupo(Guid usuarioId)
        {
            var membro = _membros.FirstOrDefault(m => m.UsuarioId == usuarioId);
            Guard.Against<DomainException>(membro is null, "Você não é membro deste grupo.");

            Guard.Against<DomainException>(
                _membros.Count == 1,
                "Você é o único membro do grupo e não pode sair.");

            if (membro!.Role == PapelMembro.Administrador)
            {
                var proximo = _membros
                    .Where(m => m.UsuarioId != usuarioId)
                    .OrderBy(m => m.DataCriacao)
                    .First();

                proximo.MudarPapel(PapelMembro.Administrador);
            }

            _membros.Remove(membro);
            DataAtualizacao = DateTime.UtcNow;

            return membro;
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