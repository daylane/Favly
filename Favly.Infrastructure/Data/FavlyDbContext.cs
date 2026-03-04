using Favly.Domain.Common.Base;
using Favly.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Wolverine;
using Wolverine.EntityFrameworkCore;

namespace Favly.Infrastructure.Data
{
    public class FavlyDbContext : DbContext
    {
        private readonly IMessageBus _bus; // Wolverine
        public FavlyDbContext(DbContextOptions<FavlyDbContext> options, IMessageBus bus): base(options)
        {
            _bus = bus;
        }

        public DbSet<Grupo> Grupos => Set<Grupo>();
        public DbSet<Membro> Membros => Set<Membro>();
        public DbSet<Tarefa> Tarefas => Set<Tarefa>();
        public DbSet<Pagamento> Pagamentos => Set<Pagamento>();
        public DbSet<Convite> Convites => Set<Convite>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FavlyDbContext).Assembly);

            modelBuilder.MapWolverineEnvelopeStorage();

            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            var entries = ChangeTracker.Entries<Entity>()
                .Where(x => x.Entity.DomainEvents.Any())
                .ToList();

            var eventos = entries.SelectMany(x => x.Entity.DomainEvents).ToList();

            entries.ForEach(x => x.Entity.ClearDomainEvents());

            var result = await base.SaveChangesAsync(ct);

            foreach (var @event in eventos)
            {
                await _bus.PublishAsync(@event);
            }

            return result;
        }

    }
}
