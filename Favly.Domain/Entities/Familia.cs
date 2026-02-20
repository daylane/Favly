using Favly.Domain.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Domain.Entities
{
    internal class Familia : Entity
    {
        public string Nome { get; private set; }
        public string Convite { get; private set; }

    }
}
