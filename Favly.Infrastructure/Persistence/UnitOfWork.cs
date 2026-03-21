using Favly.Domain.Interfaces;
using Favly.Infrastructure.Data;

namespace Favly.Infrastructure.Persistence
{
    public class UnitOfWork(FavlyDbContext _context) : IUnitOfWork
    {
        public Task<int> CommitAsync(CancellationToken ct = default) =>
            _context.SaveChangesAsync(ct);
    }
}
