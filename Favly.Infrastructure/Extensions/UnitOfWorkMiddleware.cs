using Favly.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Wolverine;
using Wolverine.Runtime;

namespace Favly.Infrastructure.Extensions
{
    public class UnitOfWorkMiddleware(IUnitOfWork _uow, IMessageBus _bus)
    {
        public async Task AfterAsync(CancellationToken ct)
        {
            await _uow.CommitAsync(ct);
        }
    }
}
