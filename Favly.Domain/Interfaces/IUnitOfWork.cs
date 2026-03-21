using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync(CancellationToken ct = default);
    }
}
