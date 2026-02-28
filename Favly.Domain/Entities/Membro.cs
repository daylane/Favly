using Favly.Domain.Common.Base;
using Favly.Domain.Common.Enums;
using Favly.Domain.Common.Validations;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Favly.Domain.Entities
{
    public class Membro : Entity
    {
        public Guid FamiliaId { get; private set; }
        public Guid UsuarioId { get; private set; }
        public string Apelido { get; private set; }
        public PapelMembro Role { get; private set; }
        protected Membro() { }

        internal Membro(Guid familiaId, Guid usuarioId, string apelido, PapelMembro role)
        {
            Guard.AgainstEmptyGuid(familiaId, nameof(familiaId));
            Guard.AgainstEmptyGuid(usuarioId, nameof(usuarioId));
            Guard.AgainstNullOrWhiteSpace(apelido, nameof(apelido));
            Guard.AgainstInvalidEnum<PapelMembro>(role, nameof(role));

            FamiliaId = familiaId;
            UsuarioId = usuarioId;
            Apelido = apelido;
            Role = role;
        }

        public void AlterarApelido(string novoApelido)
        {
            Guard.AgainstNullOrWhiteSpace(novoApelido, nameof(novoApelido));
            Apelido = novoApelido;
            DataAtualizacao = DateTime.UtcNow;
        }

        public void MudarPapel(PapelMembro novoPapel)
        {
            Guard.AgainstInvalidEnum<PapelMembro>(novoPapel, nameof(novoPapel));

            Role = novoPapel;
            DataAtualizacao = DateTime.UtcNow;
        }
    }
}

