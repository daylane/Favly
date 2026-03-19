using Favly.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Wolverine.Runtime;

namespace Favly.Infrastructure.Extensions
{
    public class UnitOfWorkMiddleware
    {
        private readonly IUnitOfWork _uow;

        public UnitOfWorkMiddleware(IUnitOfWork uow) => _uow = uow;

        public async Task AfterAsync(CancellationToken ct)
        {
            await _uow.CommitAsync(ct);
        }
    }
}
