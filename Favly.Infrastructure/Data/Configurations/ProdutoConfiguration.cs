using Favly.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Infrastructure.Data.Configurations
{
    public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("Produtos");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Nome).HasMaxLength(100).IsRequired();
            builder.Property(p => p.Marca).HasMaxLength(100);
            builder.Property(p => p.Unidade).HasConversion<int>().IsRequired();
            builder.Property(p => p.QuantidadeAtual).HasPrecision(18, 3).IsRequired();
            builder.Property(p => p.QuantidadeMinima).HasPrecision(18, 3).IsRequired();
            builder.Property(p => p.UltimoPreco).HasPrecision(18, 2);
            builder.Ignore(p => p.EstoqueAbaixoDoMinimo); // propriedade calculada
        }
    }
}
