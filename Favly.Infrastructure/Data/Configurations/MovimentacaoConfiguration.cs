using Favly.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Infrastructure.Data.Configurations
{
    public class MovimentacaoConfiguration : IEntityTypeConfiguration<Movimentacao>
    {
        public void Configure(EntityTypeBuilder<Movimentacao> builder)
        {
            builder.ToTable("Movimentacoes");
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Tipo).HasConversion<int>().IsRequired();
            builder.Property(m => m.Quantidade).HasPrecision(18, 3).IsRequired();
            builder.Property(m => m.Preco).HasPrecision(18, 2);
            builder.Property(m => m.Observacao).HasMaxLength(500);
            builder.Property(m => m.MercadoId).IsRequired(false);
        }
    }
}
