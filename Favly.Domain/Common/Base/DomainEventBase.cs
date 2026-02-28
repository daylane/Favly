using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Domain.Common.Base
{
    public abstract record class DomainEventBase : IDomainEvent
    {
        public DateTime OcorreuEm { get; protected set; } = DateTime.UtcNow;    
    }
}
