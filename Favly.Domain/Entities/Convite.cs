using Favly.Domain.Common.Base;
using Favly.Domain.Common.Enums;
using Favly.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Favly.Domain.Entities
{
    public  class Convite : Entity
    {
        public Guid FamiliaId { get; private set; }
        public EmailUsuario EmailConvidado { get; private set; }
        public StatusConvite Status {  get; private set; }
    }
}
