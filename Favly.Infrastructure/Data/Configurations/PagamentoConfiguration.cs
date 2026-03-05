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
            builder.HasKey(p => p.Id); // Sempre bom reforçar a PK

            // 1. Mapeando o Money VO (Já estava ok)
            builder.OwnsOne(p => p.Valor, v =>
            {
                v.Property(x => x.Valor).HasColumnName("Montante").HasPrecision(18, 2);
                v.Property(x => x.Moeda).HasColumnName("Moeda").HasMaxLength(3).HasDefaultValue("BRL");
            });

            // 2. ADICIONE ISSO AQUI: Mapeando a Recorrência no Pagamento
            builder.OwnsOne(p => p.Recorrencia, r =>
            {
                r.Property(x => x.Frequencia).HasColumnName("RecorrenciaTipo");
                r.Property(x => x.Intervalo).HasColumnName("RecorrenciaIntervalo");
            });

            // Configurações adicionais recomendadas
            builder.Property(p => p.Titulo).HasMaxLength(150).IsRequired();
        }
    }
}
