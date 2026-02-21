using Favly.Domain.Common.Base;
using Favly.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Domain.Entities
{
    internal class Membros : Entity
    {
        public string Nome { get; private set; }
        public Papel Permissao { get; private set; }
        public int FamiliaId { get; private set; }
        public int UsuarioId { get; private set; }

        public virtual Grupo Familia { get; private set; }
    }
}
