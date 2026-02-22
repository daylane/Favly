using Favly.Domain.Common.Base;
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
    }
}