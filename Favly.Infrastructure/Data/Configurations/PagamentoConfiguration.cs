using Favly.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Infrastructure.Data.Configurations
{
    public class PagamentoConfiguration : IEntityTypeConfiguration<Pagamento>
    {
        public void Configure(EntityTypeBuilder<Pagamento> builder)
        {
            builder.ToTable("Pagamentos");

            // Mapeando o Money VO (Valor e Moeda)
            builder.OwnsOne(p => p.Valor, v =>
            {
                v.Property(x => x.Valor).HasColumnName("Montante").HasPrecision(18, 2);
                v.Property(x => x.Moeda).HasColumnName("Moeda").HasMaxLength(3).HasDefaultValue("BRL");
            });
        }
    }
}
