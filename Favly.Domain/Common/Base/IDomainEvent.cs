using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Domain.Common.Base
{
    public interface IDomainEvent : INotification
    {
        DateTime OcorreuEm { get; }
    }
}
