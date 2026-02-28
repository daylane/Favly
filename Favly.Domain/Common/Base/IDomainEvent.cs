using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Domain.Common.Base
{
    public interface IDomainEvent
    {
        public DateTime OcorreuEm { get; } 
    }
}
