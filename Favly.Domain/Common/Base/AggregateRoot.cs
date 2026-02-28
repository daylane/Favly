using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Domain.Common.Base
{
    public abstract class AggregateRoot : Entity
    {
        protected AggregateRoot() : base() { }

        protected AggregateRoot(Guid id) : base(id) { }
    }
}
