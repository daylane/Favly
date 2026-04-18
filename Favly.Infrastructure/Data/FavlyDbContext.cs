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

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Convite> Convites { get; set; }
        public DbSet<Membro> Membros { get; set; }
        public DbSet<TokenResetSenha> TokensResetSenha => Set<TokenResetSenha>();
        public DbSet<Categoria> Categorias => Set<Categoria>();
        public DbSet<Mercado> Mercados => Set<Mercado>();
        public DbSet<Produto> Produtos => Set<Produto>();
        public DbSet<Movimentacao> Movimentacoes => Set<Movimentacao>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FavlyDbContext).Assembly);

            modelBuilder.MapWolverineEnvelopeStorage();

            base.OnModelCreating(modelBuilder);
        }
    }
}
