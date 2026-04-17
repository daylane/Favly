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
        public FavlyDbContext(DbContextOptions<FavlyDbContext> options) : base(options) { }

        public DbSet<Tarefa> Tarefas { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Convite> Convites { get; set; }
        public DbSet<Membro> Membros { get; set; }
        public DbSet<TokenResetSenha> TokensResetSenha => Set<TokenResetSenha>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FavlyDbContext).Assembly);

            modelBuilder.MapWolverineEnvelopeStorage();

            base.OnModelCreating(modelBuilder);
        }
    }
}
